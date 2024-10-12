// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Data.Models.Base;
using InfiniLore.Server.Data.Models.UserData;
using Microsoft.EntityFrameworkCore;

namespace InfiniLore.Server.Contracts.Data;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public interface IInfiniLoreDbContext {
    public DbSet<LoreScopeModel> LoreScopes { get; init; }
    public DbSet<MultiverseModel> Multiverses { get; init; }
    public DbSet<UniverseModel> Universes { get; init; }

    #region AuditLogs
    public DbSet<AuditLog<LoreScopeModel>> LoreScopeAuditLogs { get; init; }
    public DbSet<AuditLog<MultiverseModel>> MultiverseAuditLogs { get; init; }
    public DbSet<AuditLog<UniverseModel>> UniverseAuditLogs { get; init; }
    #endregion
}
