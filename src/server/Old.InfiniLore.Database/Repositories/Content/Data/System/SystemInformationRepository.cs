// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using CodeOfChaos.Types.UnitOfWork;
using Old.InfiniLore.Contracts.Database.Repositories.Content.Data.System;
using Microsoft.Extensions.DependencyInjection;

namespace Old.InfiniLore.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ISystemInformationRepository>(ServiceLifetime.Scoped)]
public class SystemInformationRepository: UnitOfWorkRepository<ContentDbContext>, ISystemInformationRepository;
