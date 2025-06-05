// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.CliArgsParser;
using Docker.DotNet;
using Docker.DotNet.Models;
using Microsoft.Extensions.Logging;

namespace DevTools.InfiniLore.Commands.RemoveContainers;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[CliData("containers-remove")]
public partial class RemoveContainerCommand(
    ILogger<RemoveContainerCommand> logger
) : ICliCommand<RemoveContainerParameters> {

    private static readonly Dictionary<string, IDictionary<string, bool>> ListFilters = new() {
        ["label"] = new Dictionary<string, bool> { ["com.docker.compose.project=infinilore-dev"] = true }
    };
    private static DockerClientConfiguration DockerClientConfiguration { get; } = new();
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async ValueTask ExecuteAsync(RemoveContainerParameters parameters, CancellationToken ct = new()) {
        try {
            DockerClient client = DockerClientConfiguration.CreateClient();

            var listParameters = new ContainersListParameters {
                All = true, // Include stopped containers
                Filters = ListFilters
            };
            IList<ContainerListResponse> containers = await client.Containers.ListContainersAsync(listParameters, ct);

            if (!containers.Any()) {
                logger.Information("No InfiniLore development containers found");
                return;
            }

            await Task.WhenAll(containers
                .Select(container => RemoveContainerAsync(client, container, ct))
            );

            logger.Information("Successfully removed all InfiniLore development containers");
        }
        catch (Exception ex) {
            logger.Error(ex, "Failed to remove containers");
            throw;
        }
    }
    
    private async Task RemoveContainerAsync(DockerClient client, ContainerListResponse container, CancellationToken ct) {
        try {
            logger.Information("Removing container {ContainerId} ({Names})", container.ID[..12], string.Join(", ", container.Names));

            var removeParameters = new ContainerRemoveParameters { Force = true, RemoveVolumes = true };
            await client.Containers.RemoveContainerAsync(container.ID, removeParameters, ct);
        }
        catch (Exception e) {
            logger.Warning(e, "Failed to remove container {ContainerId} ({Names})", container.ID[..12], string.Join(", ", container.Names));
                    
        }
    }
}
