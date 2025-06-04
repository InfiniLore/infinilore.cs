// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
import { SecureStorage, TokenData } from "./Contracts";

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
export class SecureStorageService implements SecureStorage {
    private static readonly DB_NAME = "JwtStorage";
    private static readonly DB_VERSION = 3;
    private static readonly STORE_NAME = "tokens";

    private async _openDatabaseAsync(): Promise<IDBDatabase> {
        return new Promise((resolve, reject) => {
            const request: IDBOpenDBRequest = indexedDB.open(SecureStorageService.DB_NAME, SecureStorageService.DB_VERSION);

            request.onupgradeneeded = (event: IDBVersionChangeEvent) => {
                const db: IDBDatabase = (event.target as IDBOpenDBRequest).result;
                console.log("Upgrading database (v3). Existing object stores:", db.objectStoreNames);
                if (!db.objectStoreNames.contains(SecureStorageService.STORE_NAME)) {
                    console.log("Creating object store 'tokens'");
                    db.createObjectStore(SecureStorageService.STORE_NAME, { keyPath: "id" });
                }
            };

            request.onsuccess = (event: Event) => {
                const db: IDBDatabase = (event.target as IDBOpenDBRequest).result;
                console.log("Database opened successfully. Object stores:", db.objectStoreNames);
                if (!db.objectStoreNames.contains(SecureStorageService.STORE_NAME)) {
                    console.warn("Object store 'tokens' not found even after opening DB.");
                }
                resolve(db);
            };

            request.onerror = (event: Event) => {
                reject("Failed to open IndexedDB: " + (event.target as IDBOpenDBRequest).error);
            };
        });
    }

    private async _executeTransactionAsync(
        storeName: string,
        mode: IDBTransactionMode,
        executeCallback: (store: IDBObjectStore) => IDBRequest
    ): Promise<any> {
        const db = await this._openDatabaseAsync();

        if (!db.objectStoreNames.contains(storeName)) {
            console.error(`Object store '${storeName}' not found.`);
            return Promise.reject(`Object store '${storeName}' does not exist.`);
        }

        const transaction: IDBTransaction = db.transaction([storeName], mode);
        const store: IDBObjectStore = transaction.objectStore(storeName);

        return new Promise((resolve, reject) => {
            const request: IDBRequest = executeCallback(store);

            request.onsuccess = (event: Event) => {
                resolve((event.target as IDBRequest).result || null);
            };

            request.onerror = (event: Event) => {
                reject("Transaction failed: " + (event.target as IDBRequest).error);
            };
        });
    }

    public async saveTokenAsync(key: string, value: string, expiresAt: Date): Promise<any> {
        return await this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readwrite", (store) =>
            store.put({ id: key, value: value, expiresAt: expiresAt })
        );
    }

    public async getTokenAsync(key: string): Promise<TokenData | null> {
        return await this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readonly", (store) =>
            store.get(key)
        );
    }

    public async removeTokenAsync(key: string): Promise<any> {
        return await this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readwrite", (store) =>
            store.delete(key)
        );
    }
}
