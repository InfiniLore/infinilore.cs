// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Modules.Core.Server.Database;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public readonly record struct QueryConfig(
    bool OptionalInclude = false,
    bool Reverse = false,
    bool RetrieveSoftDeleted = false
) {
    public static QueryConfig Default => new();
    public static QueryConfig WithOptional => new(OptionalInclude: true);
    public static QueryConfig WithReverse => new(Reverse: true);
    public static QueryConfig WithRetrieveSoftDeleted => new(RetrieveSoftDeleted: true);
    
    public static QueryConfig From<T>(T config) where T : IHasOptionalInclude, IHasReverse, IHasRetrieveSoftDeleted
        => new(config.OptionalInclude, config.Reverse, config.RetrieveSoftDeleted);
    public static QueryConfig From(IHasOptionalInclude config) => new(config.OptionalInclude);
    public static QueryConfig From(IHasReverse config) => new(config.Reverse);
    public static QueryConfig From(IHasRetrieveSoftDeleted config) => new(config.RetrieveSoftDeleted);
}

public interface IHasReverse {
    bool Reverse { get; }
}

public interface IHasOptionalInclude {
    bool OptionalInclude { get; }
}

public interface IHasRetrieveSoftDeleted {
    bool RetrieveSoftDeleted { get; }
}