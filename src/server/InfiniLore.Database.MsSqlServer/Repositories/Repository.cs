// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using InfiniLore.Server.Contracts.Database;

namespace InfiniLore.Database.MsSqlServer.Repositories;

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
    public void Attach(IUnitOfWork unitOfWork) {
        if (_unitOfWork is not null) return;

        UnitOfWork = unitOfWork;
    }

    public void Detach(IUnitOfWork unitOfWork) {
        if (_unitOfWork is null || _unitOfWork == unitOfWork) return;
     
        _unitOfWork = null;
    }
    
    protected virtual IQueryable<T> IncludeOnGet(IQueryable<T> query) => query;
}
