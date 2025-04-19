// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Contracts.Database.Repositories.Data.User;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Services.Messaging.Notifications.Data.Project;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Messaging.Commands.Data.Project;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFileAddOrUpdateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILogger<MarkdownFileAddOrUpdateHandler> logger, IValidator<MarkdownFile> validator) : CommandHandler<MarkdownFileAddOrUpdateRequest, MessageResponse<Guid>> {
    public override async Task<MessageResponse<Guid>> ExecuteAsync(MarkdownFileAddOrUpdateRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var markdownFileRepo = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);
        var loreScopeRepo = await unitOfWork.GetRepositoryAsync<ILoreScopeRepository>(ct);

        Result userIdExistsResult = await loreScopeRepo.IsIdTakenAsync(command.LoreScopeId, ct);
        if (userIdExistsResult.IsError) return MessageResponse<Guid>.FromErrorString("Owner id does not exist");

        // Create a new markdownFIle based on the request
        var markdownFile = new MarkdownFile {
            Id = command.MarkdownFileId != Guid.Empty ? command.MarkdownFileId : Guid.CreateVersion7(),
            Name = command.FileName,
            LoreScopeId = command.LoreScopeId,
            Source = command.Source
        };

        // Validate the markdownFIle model
        ValidationResult validationResult = await validator.ValidateAsync(markdownFile, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MessageResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await markdownFileRepo.AddOrUpdateAsync(markdownFile, ct);
        if (result.IsError) return MessageResponse<Guid>.FromErrorString("Failed to save user to database");

        await new NewMarkdownFileCreatedEvent(markdownFile.Id).PublishAsync(Mode.WaitForAll, ct);
        logger.LogInformation("MarkdownFile created: {MarkdownFileId}, notification sent", markdownFile.Id);
        return markdownFile.Id;
    }
}
