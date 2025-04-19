// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraEngine.Unions;
using CodeOfChaos.Types.UnitOfWork;
using FastEndpoints;
using FluentValidation;
using FluentValidation.Results;
using InfiniLore.Server.Contracts.Database.Repositories.Account;
using InfiniLore.Server.Contracts.Database.Repositories.Data.Project;
using InfiniLore.Server.Database.Models.Data.Project;
using InfiniLore.Server.Services.Messaging.Notifications.Data.Project;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace InfiniLore.Server.Services.Messaging.Commands.Data.Project;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[UsedImplicitly]
public class MarkdownFileCreateHandler(IUnitOfWorkFactory unitOfWorkFactory, ILogger<MarkdownFileCreateHandler> logger, IValidator<MarkdownFile> validator) : CommandHandler<MarkdownFileCreateRequest, MessageResponse<Guid>> {
    public override async Task<MessageResponse<Guid>> ExecuteAsync(MarkdownFileCreateRequest command, CancellationToken ct = new()) {
        await using IUnitOfWork unitOfWork = unitOfWorkFactory.Create();
        var markdownFileRepo = await unitOfWork.GetRepositoryAsync<IMarkdownFileRepository>(ct);
        var userRepo = await unitOfWork.GetRepositoryAsync<IUserRepository>(ct);

        Result markdownFileNameTakenResult = await markdownFileRepo.IsFileNameTakenAsync(command.FileName, command.LoreScopeId, ct);
        Result userIdExistsResult = await userRepo.IsIdTakenAsync(command.LoreScopeId, ct);
        if (markdownFileNameTakenResult.TryGetState(out bool isTaken) && isTaken) return MessageResponse<Guid>.FromErrorString("MarkdownFile name already taken for this user");
        if (userIdExistsResult.IsError) return MessageResponse<Guid>.FromErrorString("Owner id does not exist");

        // Create a new lorescope based on the request
        var markdownFile = new MarkdownFile {
            Name = command.FileName,
            LoreScopeId = command.LoreScopeId,
            Source = command.Source
        };

        // Validate the lorescope model
        ValidationResult validationResult = await validator.ValidateAsync(markdownFile, ct);
        if (!validationResult.IsValid) {
            logger.Warning("Validation failed: {Reason}", validationResult.Errors);
            return MessageResponse<Guid>.FromErrorString("Validation failed");
        }

        // Save to Db
        Result result = await markdownFileRepo.AddAsync(markdownFile, ct);
        if (result.IsError) return MessageResponse<Guid>.FromErrorString("Failed to save user to database");

        await new NewMarkdownFileCreatedEvent(markdownFile.Id).PublishAsync(Mode.WaitForAll, ct);
        logger.LogInformation("MarkdownFile created: {MarkdownFileId}, notification sent", markdownFile.Id);
        return markdownFile.Id;
    }
}
