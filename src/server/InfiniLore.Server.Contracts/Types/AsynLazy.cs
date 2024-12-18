// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Contracts.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AsyncLazy<T>(Func<CancellationToken, Task<T>> valueFactory) : IAsyncDisposable {
    private readonly Lock _lock = new();
    private Task<T>? _value;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<T> GetValueAsync(CancellationToken ct = default) {
        if (_value != null) return await _value;

        lock (_lock) {
            _value ??= valueFactory(ct);// Double check locking
        }

        return await _value;
    }
    
    public async ValueTask DisposeAsync() {
        T data = await GetValueAsync();
        switch (data) {
            case IAsyncDisposable asyncDisposable: await asyncDisposable.DisposeAsync(); break;
            case IDisposable disposable: disposable.Dispose(); break;
        }
        
        _value = null;
        GC.SuppressFinalize(this);
    }
}
