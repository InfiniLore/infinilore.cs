// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using FluentValidation;
using InfiniLore.Server.Modules.Core.Database;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Server.Modules.LoreScopes.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<IValidator<LoreScopeDescriptionModel>>(ServiceLifetime.Singleton)]
public class LoreScopeDescriptionValidator : MarkdownDocumentValidator<LoreScopeModel, LoreScopeDescriptionModel>;
