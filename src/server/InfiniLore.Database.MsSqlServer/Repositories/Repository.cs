// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database;

namespace InfiniLore.Database.Repositories;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class Repository<T> : IRepository {
    private IUnitOfWork? _unitOfWork;
    protected IUnitOfWork UnitOfWork {
        get => _unitOfWork ?? throw new InvalidOperationException("UnitOfWork is not set");
        private set => _unitOfWork = value;
    }
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public bool TryAttach(IUnitOfWork unitOfWork) {
        if (_unitOfWork is not null) return false;
        UnitOfWork = unitOfWork;
        return true;
    }

    public bool TryDetach(IUnitOfWork unitOfWork) {
        if (_unitOfWork is null || _unitOfWork == unitOfWork) return false;
        _unitOfWork = null;
        return true;
    }
    
    protected virtual IQueryable<T> IncludeOnGet(IQueryable<T> query) => query;
}
