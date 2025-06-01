// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using JetBrains.Annotations;
using LibGit2Sharp;
using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using NuGetRepository=NuGet.Protocol.Core.Types.Repository;
using GitRepository=LibGit2Sharp.Repository;

namespace DevTools.InfiniLore.Commands.DownloadRepo;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
[CliData("download-repo")]
public partial class DownloadRepoCommand : ICliCommand<DownloadRepoParameters> {
    private readonly SourceCacheContext Cache = new();
    private readonly SourceRepository Repo = NuGetRepository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

    [GeneratedRegex(@"^(?!Tools).*\.csproj$")]
    private static partial Regex ExcludeToolsRegex { get; }

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(DownloadRepoParameters parameters, CancellationToken ct = default) {
        string[] paths = GetProjectPaths(parameters);
        string tempDirectory = Path.Combine(parameters.Root, ".temp");

        // Step 1: Clean up temporary and existing repositories
        if (Directory.Exists(tempDirectory)) CleanDirectory(tempDirectory);

        // Step 2: Backup or log existing state (optional)
        BackupRepositories(parameters);

        // Step 3: Collect the projects to override
        ProjectData[][] projectDatas = await Task.WhenAll(paths.Select(p => GetExternalProjectsAsync(p)));
        ProjectData[] projects = projectDatas.SelectMany(p => p).DistinctBy(p => p.Name).ToArray();

        // Step 4: Fetch project data with links
        IEnumerable<Task<ProjectData>> tasks = projects.Select(async project => project + await GetGithubLinkAsync(project));
        ProjectData[] results = await Task.WhenAll(tasks);
        foreach (ProjectData project in results) {
            Console.WriteLine($"{project.Name} - {project.Version} - {project.GithubLink}");
        }

        // Step 5: Download all repositories (overrides handled in ExtractPackage)
        IEnumerable<Task> downloadTasks = results.Select(project => DownloadPackageAsync(parameters, project));
        await Task.WhenAll(downloadTasks);

        // Step 6: Extract files
        foreach (ProjectData project in results) ExtractPackage(parameters, project);

        // Step 7: Update solution and dependencies
        if (parameters.LinkToSolution) {
            await AddProjectsToSolutionAsync(parameters, projects);
            await CleanupCsprojFilesAsync(parameters, projects);
            // await RemapDependenciesAsync(parameters, projects);
        }
    }

    private static void CleanDirectory(string directoryPath) {
        if (!Directory.Exists(directoryPath)) return;

        var directoryInfo = new DirectoryInfo(directoryPath);

        try {
            // Remove read-only attributes from all files
            foreach (FileInfo file in directoryInfo.GetFiles("*", SearchOption.AllDirectories)) {
                if (file.IsReadOnly) {
                    file.IsReadOnly = false;// Remove read-only attribute
                }

                file.Delete();// Delete the file
            }

            // Recursively delete all subdirectories
            foreach (DirectoryInfo subDirectory in directoryInfo.GetDirectories()) {
                CleanDirectory(subDirectory.FullName);// Recursive cleaning
            }

            // Finally, delete the directory itself
            Directory.Delete(directoryPath, true);
            Console.WriteLine($"Successfully cleaned: {directoryPath}");
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to clean directory {directoryPath}: {ex.Message}");
        }
    }


    private static void BackupRepositories(DownloadRepoParameters parameters) {
        string backupDir = Path.Combine(parameters.Root, "backup");
        Directory.CreateDirectory(backupDir);

        foreach (string folder in Directory.EnumerateDirectories(Path.Combine(parameters.Root, "src"))) {
            string backupPath = Path.Combine(backupDir, Path.GetFileName(folder));
            if (Directory.Exists(backupPath)) continue;

            Directory.Move(folder, backupPath);
            Console.WriteLine($"Backed up {folder} to {backupPath}");
        }
    }


    public static string[] GetProjectPaths(DownloadRepoParameters parameters) {
        var paths = new List<string>();

        string sourceDirectory = Path.Combine(parameters.Root, "src");
        string testsDirectory = Path.Combine(parameters.Root, "tests");

        Console.WriteLine($"Source Directory: {sourceDirectory}");
        Console.WriteLine($"Tests Directory: {testsDirectory}");

        // Get source projects
        string[] sourceProjects = Directory.GetFiles(sourceDirectory, "*.csproj", SearchOption.AllDirectories)
            .Where(file => ExcludeToolsRegex.IsMatch(Path.GetFileName(file)))
            .ToArray();

        // Get test projects
        string[] testProjects = Directory.GetFiles(testsDirectory, "*.csproj", SearchOption.AllDirectories)
            .Where(file => ExcludeToolsRegex.IsMatch(Path.GetFileName(file)))
            .ToArray();

        paths.AddRange(sourceProjects);
        paths.AddRange(testProjects);

        return paths.ToArray();
    }

    private static async Task<ProjectData[]> GetExternalProjectsAsync(string projectPath, CancellationToken ct = default) {
        try {
            // Resolve full path and ensure the file exists
            string fullPath = Path.GetFullPath(projectPath);
            if (!File.Exists(fullPath)) return [];

            // Asynchronously load the XML document
            await using FileStream stream = File.OpenRead(fullPath);
            using var reader = XmlReader.Create(stream, new XmlReaderSettings { Async = true });
            XDocument doc = await XDocument.LoadAsync(reader, LoadOptions.PreserveWhitespace, ct);

            // Define allowed projects
            string[] allowedProjects = ["CodeOfChaos", "AterraEngine", "InfiniLore"];

            // Extract and return allowed projects
            return doc.Descendants("PackageReference")
                .Where(e => allowedProjects.Contains(e.Attribute("Include")?.Value.Split('.')[0] ?? string.Empty))
                .Select(e => new ProjectData(
                    e.Attribute("Include")?.Value ?? string.Empty,
                    e.Attribute("Version")?.Value ?? string.Empty))
                .ToArray();
        }
        catch (Exception ex) {
            Console.WriteLine($"Error processing project file '{projectPath}': {ex}");
            return [];
        }
    }

    private async Task<string?> GetGithubLinkAsync(ProjectData project, CancellationToken ct = default) {
        var resource = await Repo.GetResourceAsync<PackageMetadataResource>(ct);
        IEnumerable<IPackageSearchMetadata>? metadata = await resource.GetMetadataAsync(
            project.Name,
            false,
            false,
            Cache,
            NullLogger.Instance,
            ct
        );


        return metadata?.FirstOrDefault(m => m.Identity.Version.OriginalVersion == project.Version)?.ProjectUrl?.AbsoluteUri;
    }

    private static async Task DownloadPackageAsync(DownloadRepoParameters parameters, ProjectData data, CancellationToken ct = default) {
        // Define the local output folder for downloaded packages
        string outputDirectory = Path.Combine(parameters.Root, ".temp/packages", data.Name, data.Version);
        Directory.CreateDirectory(outputDirectory);

        await Task.Run(action: () => {
            var cloneOptions = new CloneOptions {
                Checkout = false// Disable default checkout
            };

            GitRepository.Clone(data.GithubLink, outputDirectory, cloneOptions);
            using var repo = new GitRepository(outputDirectory);

            Tag? tag = repo.Tags[$"v{data.Version}"];
            if (tag == null) {
                throw new NotFoundException($"Project : {data.Name} at {data.GithubLink} \n Tag 'v{data.Version}' not found in the repository.");
            }

            // Checkout the tag
            var commit = tag.Target as Commit;
            LibGit2Sharp.Commands.Checkout(repo, commit);

            Console.WriteLine($"Downloaded {data.Name} - {data.Version}");
        }, ct);
    }

    private static void ExtractPackage(DownloadRepoParameters parameters, ProjectData data) {
        string gitDirectory = Path.Combine(parameters.Root, ".temp/packages", data.Name, data.Version);

        // Handle special case for Old.InfiniLore.Server.Types
        string projectDirectory = data.Name == "Old.InfiniLore.Server.Types"
            ? Path.Combine(gitDirectory, "src", "server", data.Name)
            : Path.Combine(gitDirectory, "src", data.Name);

        string projectTestsDirectory = Path.Combine(gitDirectory, "tests", $"Tests.{data.Name}");
        string fullPath = Path.GetFullPath(projectDirectory);
        string fullPathTests = Path.GetFullPath(projectTestsDirectory);

        string outputDirectory = Path.Combine(parameters.Root, $"{parameters.OutputFolder}src", data.Name);
        string outputTestsDirectory = Path.Combine(parameters.Root, $"{parameters.OutputFolder}tests", $"Tests.{data.Name}");

        // Attempt to delete the target directories if they exist
        DeleteDirectoryIfExists(outputDirectory);
        DeleteDirectoryIfExists(outputTestsDirectory);

        try {
            // Create the destination directories
            Directory.CreateDirectory(outputDirectory);
            Directory.CreateDirectory(outputTestsDirectory);

            // Move the source directories to the destination
            MoveDirectory(fullPath, outputDirectory);
            Console.WriteLine($"Successfully moved {fullPath} to {outputDirectory}");

            MoveDirectory(fullPathTests, outputTestsDirectory);
            Console.WriteLine($"Successfully moved {fullPathTests} to {outputTestsDirectory}");
        }
        catch (UnauthorizedAccessException ex) {
            Console.WriteLine($"Access denied: {ex.Message}");
        }
        catch (IOException ex) {
            Console.WriteLine($"IOException occurred: {ex.Message}");
        }
        catch (Exception ex) {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }

    private static void DeleteDirectoryIfExists(string directoryPath) {
        if (!Directory.Exists(directoryPath)) return;

        Console.WriteLine($"The target directory {directoryPath} already exists. Attempting to delete...");
        try {
            Directory.Delete(directoryPath, true);// Delete the directory if it exists
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to delete existing directory {directoryPath}: {ex.Message}");
        }
    }

    private static void MoveDirectory(string sourceDir, string destDir) {
        string backupDir = string.Empty;

        try {
            // Ensure the destination directory does not already exist
            if (Directory.Exists(destDir)) {
                // Rename existing destination folder before moving the new one
                backupDir = destDir + "_backup_" + Guid.NewGuid();
                Directory.Move(destDir, backupDir);
                Console.WriteLine($"Renamed existing directory {destDir} to {backupDir}");
            }

            // Move the source directory to the destination
            Directory.Move(sourceDir, destDir);
            Console.WriteLine($"Moved directory from {sourceDir} to {destDir}");

            // If the move was successful, delete the backup directory
            if (Directory.Exists(backupDir)) {
                Directory.Delete(backupDir, true);
                Console.WriteLine($"Deleted backup directory {backupDir}");
            }
        }
        catch (Exception ex) {
            Console.WriteLine($"Failed to move directory from {sourceDir} to {destDir}: {ex.Message}");

            // In case of failure, ensure backup directory is deleted if it exists
            if (!string.IsNullOrEmpty(backupDir) && Directory.Exists(backupDir)) {
                Directory.Delete(backupDir, true);
                Console.WriteLine($"Deleted backup directory {backupDir}");
            }
        }
    }

    private static async ValueTask AddProjectsToSolutionAsync(DownloadRepoParameters parameters, ProjectData[] projects) {
        string solutionFilePath = Path.Combine(parameters.Root, parameters.SolutionFile);
        if (!File.Exists(solutionFilePath)) {
            Console.WriteLine($"Solution file {solutionFilePath} not found.");
            return;
        }

        foreach (ProjectData project in projects) {
            string projectFilePath = Path.Combine(parameters.Root, $"{parameters.OutputFolder}src", project.Name, $"{project.Name}.csproj");
            string testFilePath = Path.Combine(parameters.Root, $"{parameters.OutputFolder}tests", $"Tests.{project.Name}", $"Tests.{project.Name}.csproj");

            foreach ((string path, string slnFolder) in new[] { (projectFilePath, "src-external"), (testFilePath, "tests") }) {
                if (!File.Exists(path)) {
                    Console.WriteLine($"Project file {path} not found for {project.Name}");
                    continue;
                }

                Console.WriteLine($"Adding {project.Name} to solution...");

                // Run a command to add the project to the solution file
                var startInfo = new ProcessStartInfo {
                    FileName = "dotnet",
                    Arguments = $"sln \"{solutionFilePath}\" add \"{path}\" --solution-folder \"{slnFolder}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using Process? process = Process.Start(startInfo);
                if (process == null) continue;

                string output = await process.StandardOutput.ReadToEndAsync();
                Console.WriteLine(output);
                await process.WaitForExitAsync();
            }

        }
    }

    private static async ValueTask CleanupCsprojFilesAsync(DownloadRepoParameters parameters, ProjectData[] projects) {
        var settings = new XmlWriterSettings {
            Indent = true,
            IndentChars = "    ",
            Async = true,
            OmitXmlDeclaration = true,
            NewLineOnAttributes = false// Keeps attributes on the same line
        };

        foreach (ProjectData project in projects) {
            string projectFilePath = Path.Combine(parameters.Root, $"{parameters.OutputFolder}src", project.Name, $"{project.Name}.csproj");
            string testFilePath = Path.Combine(parameters.Root, $"{parameters.OutputFolder}tests", $"Tests.{project.Name}", $"Tests.{project.Name}.csproj");

            foreach (string path in new[] { projectFilePath, testFilePath }) {
                if (!File.Exists(path)) {
                    Console.WriteLine($"File not found: {path}");
                    continue;
                }

                // LOAD
                XDocument doc;
                await using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
                    doc = await XDocument.LoadAsync(stream, LoadOptions.PreserveWhitespace, CancellationToken.None);
                }

                // do stuff
                XElement? propertyGroup = doc.Descendants("PropertyGroup").FirstOrDefault();
                if (propertyGroup != null) {
                    propertyGroup.Descendants("PackageLicenseFile").Remove();
                    propertyGroup.Descendants("PackageReadmeFile").Remove();
                    propertyGroup.Descendants("PackageIcon").Remove();
                }

                List<XElement> itemGroups = doc.Descendants("ItemGroup")
                    .Where(group => group.Attributes("Label").FirstOrDefault()?.Value != "InternalsVisibleTo")
                    .ToList();

                foreach (XElement itemGroup in itemGroups) {
                    List<XElement> noneElements = itemGroup.Descendants("None")
                        .Where(e =>
                            e.Attribute("Include")?.Value.Contains("LICENSE") == true
                            || e.Attribute("Include")?.Value.Contains("README.md") == true
                            || e.Attribute("Include")?.Value.Contains("icon.png") == true
                        )
                        .ToList();

                    foreach (XElement noneElement in noneElements) {
                        noneElement.Remove();
                    }

                    // Remove the item group if it becomes empty
                    if (!itemGroup.HasElements) {
                        itemGroup.Remove();
                    }
                }


                // SAVE
                await using (var stream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
                    await using var writer = XmlWriter.Create(stream, settings);
                    doc.Save(writer);// Save while enforcing indentation
                }

                Console.WriteLine($"Cleaned up {path}");
            }
        }
    }

    // private static async ValueTask RemapDependenciesAsync(DownloadRepoParameters parameters, ProjectData[] projects) {
    //     foreach (ProjectData project in projects) {
    //         string projectFilePath = Path.Combine(parameters.Root, $"{parameters.OutputFolder}src", project.Name, $"{project.Name}.csproj");
    //
    //         if (!File.Exists(projectFilePath)) {
    //             Console.WriteLine($"Project file not found: {projectFilePath}");
    //             continue;
    //         }
    //
    //         Console.WriteLine($"Remapping dependencies for {project.Name}...");
    //
    //         // Load the project file
    //         XDocument doc;
    //         await using (var stream = new FileStream(projectFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true)) {
    //             doc = await XDocument.LoadAsync(stream, LoadOptions.PreserveWhitespace, CancellationToken.None);
    //         }
    //
    //         // Update PackageReference versions (if applicable)
    //         IEnumerable<XElement> packageReferences = doc.Descendants("PackageReference");
    //         foreach (XElement packageReference in packageReferences) {
    //             string? packageName = packageReference.Attribute("Include")?.Value;
    //
    //             // Update logic for PackageReference
    //             ProjectData matchingProject = projects.FirstOrDefault(p => p.Name == packageName);
    //             Console.WriteLine($"Updating {packageName} to version {matchingProject.Version}...");
    //             packageReference.SetAttributeValue("Version", matchingProject.Version);
    //         }
    //
    //         // Update ProjectReference paths (if applicable)
    //         IEnumerable<XElement> projectReferences = doc.Descendants("ProjectReference");
    //         foreach (XElement projectReference in projectReferences) {
    //             string? projectPath = projectReference.Attribute("Include")?.Value;
    //             if (projectPath == null) continue;
    //
    //             // Resolve full path of the old project reference
    //             string oldProjectFullPath = Path.Combine(Path.GetDirectoryName(projectFilePath) ?? string.Empty, projectPath);
    //             string newProjectPath;
    //
    //             // Check if the project exists in the new "src" folder
    //             if (File.Exists(Path.Combine(parameters.Root, $"{parameters.OutputFolder}src", Path.GetFileNameWithoutExtension(oldProjectFullPath), Path.GetFileName(oldProjectFullPath)))) {
    //                 newProjectPath = Path.Combine("..", "..", "src", Path.GetFileNameWithoutExtension(oldProjectFullPath), Path.GetFileName(oldProjectFullPath));
    //                 Console.WriteLine($"Updating project reference path: {projectPath} -> {newProjectPath}");
    //             }
    //             else {
    //                 Console.WriteLine($"Could not resolve new path for {projectPath}. Keeping the existing reference.");
    //                 newProjectPath = projectPath;// Keep the original, unaltered path if no new path is found
    //             }
    //
    //             // Update the ProjectReference to the new location
    //             projectReference.SetAttributeValue("Include", newProjectPath);
    //
    //         }
    //
    //         // Save the updated `.csproj` file
    //         var settings = new XmlWriterSettings {
    //             Indent = true,
    //             IndentChars = "    ",
    //             Async = true,
    //             OmitXmlDeclaration = true
    //         };
    //
    //         await using (var stream = new FileStream(projectFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, true)) {
    //             await using var writer = XmlWriter.Create(stream, settings);
    //             doc.Save(writer);
    //         }
    //
    //         Console.WriteLine($"Dependencies remapped successfully for {project.Name}.");
    //     }
    // }
}
