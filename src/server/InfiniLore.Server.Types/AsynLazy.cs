// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
namespace InfiniLore.Server.Types;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public class AsyncLazy<T>(Func<CancellationToken, Task<T>> valueFactory) : IAsyncDisposable {
    private readonly SemaphoreSlim _semaphore = new(1, 1);
    private Task<T>? _value;

    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public async Task<T> GetValueAsync(CancellationToken ct = default) {
        ct.ThrowIfCancellationRequested();
        
        Task<T>? value = _value;
        if (value != null) return await value.ConfigureAwait(false);
        
        await _semaphore.WaitAsync(ct).ConfigureAwait(false);
        try {
            value = _value;
            if (value == null) {
                _value = value = valueFactory(ct);
            }
        }
        finally {
            _semaphore.Release();
        }

        return await value.ConfigureAwait(false);
    }

    public async ValueTask DisposeAsync() {
        // First do all the regular stuff
        _semaphore.Dispose();
        GC.SuppressFinalize(this);
        
        // Value might have not been loaded yet, so dispose accordingly
        if (_value == null) return;
        T data = await _value.ConfigureAwait(false);
        switch (data) {
            case IAsyncDisposable asyncDisposable: await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                break;
            case IDisposable disposable: disposable.Dispose();
                break;
        }
        
        _value = null;
    }
}
