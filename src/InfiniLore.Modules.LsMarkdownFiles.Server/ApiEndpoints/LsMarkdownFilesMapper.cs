// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Modules.Core.Server.ApiEndpoints;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableSingleton<LsMarkdownFilesMapper>]
public class LsMarkdownFilesMapper : AutoResponsesMapper<LsMarkdownFileResponse, LsMarkdownFileMapper, LsMarkdownFilesResponse, LsMarkdownFileModel>;
