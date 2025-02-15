// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using InfiniLore.Contracts.Database.Repositories.Content.Data.System;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ISystemInformationRepository>(ServiceLifetime.Scoped)]
public class SystemInformationRepository: UnitOfWorkRepository<ContentDbContext>, ISystemInformationRepository;
