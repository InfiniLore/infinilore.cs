// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using FastEndpoints;
using InfiniLore.Modules.Core.Server.Database;
using InfiniLore.Modules.Core.Shared;
using JetBrains.Annotations;

namespace InfiniLore.Modules.Core.Server.ApiEndpoints;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract record PaginatedEndpointRequest : IHasReverse, IHasPageNumber, IHasPageSize {
    private int _pageNumber = Pagination.Default.PageNumber;
    [BindFrom("pageNumber")] public int PageNumber {
        get => _pageNumber;
        [UsedImplicitly] set => _pageNumber = Math.Max(value, 0);
    } 
    
    private int _pageSize = Pagination.Default.PageSize;
    [BindFrom("pageSize")] public int PageSize {
        get => _pageSize;
        [UsedImplicitly] set => _pageSize= Math.Clamp(value, 1, Pagination.Default.PageSize);
    }
    
    [BindFrom("reverse")] public bool Reverse { get; [UsedImplicitly] set; } = false;
}
