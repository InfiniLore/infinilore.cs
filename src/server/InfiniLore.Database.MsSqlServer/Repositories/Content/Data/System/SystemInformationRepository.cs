// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using CodeOfChaos.Extensions.DependencyInjection;
using InfiniLore.Database.Models.Content.Data.System;
using InfiniLore.Server.Contracts.Database.Repositories.Content.Data.System;
using Microsoft.Extensions.DependencyInjection;

namespace InfiniLore.Database.Repositories.Content.Data.System;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
[InjectableService<ISystemInformationRepository>(ServiceLifetime.Scoped)]
public class SystemInformationRepository : Repository<SystemInformation>, ISystemInformationRepository;
