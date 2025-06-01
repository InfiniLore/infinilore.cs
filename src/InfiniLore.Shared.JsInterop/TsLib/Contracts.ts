// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
export interface TokenData {
    id: string;
    value: string;
    expiresAt: Date;
}

export interface SecureStorage {
    saveTokenAsync: (key: string, value: string, expiresAt: Date) => Promise<any>;
    getTokenAsync: (key: string) => Promise<TokenData | null>;
    removeTokenAsync: (key: string) => Promise<any>;
}

declare global {
    // noinspection JSUnusedGlobalSymbols
    interface Window {
        secureStorage: SecureStorage;
    }
}
