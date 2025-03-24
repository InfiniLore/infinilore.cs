// noinspection JSUnusedGlobalSymbols
window.secureStorage = {
    // Open (or create) the IndexedDB database with version 2 to ensure upgrade and creation of "tokens"
    _openDatabaseAsync: async () => {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open("JwtStorage", 3);

            request.onupgradeneeded = (event) => {
                const db = event.target.result;
                console.log("Upgrading database (v3). Existing object stores:", db.objectStoreNames);
                if (!db.objectStoreNames.contains("tokens")) {
                    console.log("Creating object store 'tokens'");
                    db.createObjectStore("tokens", {keyPath: "id"});
                }
            };

            request.onsuccess = (event) => {
                const db = event.target.result;
                console.log("Database opened successfully. Object stores:", db.objectStoreNames);
                if (!db.objectStoreNames.contains("tokens")) {
                    console.warn("Object store 'tokens' not found even after opening DB.");
                }
                resolve(db);
            };

            request.onerror = (event) => {
                reject("Failed to open IndexedDB: " + event.target.error);
            };
        });
    },

    // Helper to execute a transaction on a specified store
    _executeTransactionAsync: async (storeName, mode, executeCallback) => {
        const db = await window.secureStorage._openDatabaseAsync();

        if (!db.objectStoreNames.contains(storeName)) {
            console.error(`Object store '${storeName}' not found.`);
            return Promise.reject(`Object store '${storeName}' does not exist.`);
        }

        // Using an array for the store name ensures compatibility across browsers
        const transaction = db.transaction([storeName], mode);
        const store = transaction.objectStore(storeName);

        return new Promise((resolve, reject) => {
            const request = executeCallback(store);

            request.onsuccess = (event) => {
                resolve(event.target.result || null);
            };

            request.onerror = (event) => {
                reject("Transaction failed: " + event.target.error);
            };
        });
    },
    // Save the token with expiration info
    saveTokenAsync: async (key, value, expiresAt) => {
        return await window.secureStorage._executeTransactionAsync("tokens", "readwrite", (store) =>
            store.put({id: key, value: value, expiresAt: expiresAt})
        );
    },

    // Retrieve the token, including expiration info
    getTokenAsync: async (key) => {
        return await window.secureStorage._executeTransactionAsync("tokens", "readonly", (store) =>
            store.get(key)
        );
    },

    // Remove the token
    removeTokenAsync: async (key) => {
        return await window.secureStorage._executeTransactionAsync("tokens", "readwrite", (store) =>
            store.delete(key)
        );
    },
};
