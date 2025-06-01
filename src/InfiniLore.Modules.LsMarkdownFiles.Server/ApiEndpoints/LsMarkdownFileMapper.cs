// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FastEndpoints;
using InfiniLore.Server.Modules.LsMarkdownFiles.Database;

namespace InfiniLore.Modules.LsMarkdownFiles.Server.ApiEndpoints;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ------------------------------------------------- --------------------------------------------------------------------
[InjectableSingleton<LsMarkdownFileMapper>]
public class LsMarkdownFileMapper : ResponseMapper<LsMarkdownFileResponse, LsMarkdownFileModel> {
    public override LsMarkdownFileResponse FromEntity(LsMarkdownFileModel model) => new() {
        Id = model.Id,
        CreatedDate = model.CreatedDate,
        LastModifiedDate = model.LastModifiedDate,
        Name = model.Name,
        LastUserToEditId = model.LastUserToEditId,
        ResourceUrl = model.ResourceUrl,
        OwnerId = model.OwnerId,
    };
}
