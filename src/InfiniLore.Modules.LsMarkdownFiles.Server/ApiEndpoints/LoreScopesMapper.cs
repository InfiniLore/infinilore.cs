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
[InjectableSingleton<LoreScopesMapper>]
public class LoreScopesMapper : AutoResponsesMapper<LsMarkdownFileResponse, LsMarkdownFileMapper, LsMarkdownFilesResponse, LsMarkdownFileModel>;
