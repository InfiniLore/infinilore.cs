/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/InfiniLore.Server/TsLib/SecureStorage.ts":
/*!******************************************************!*\
  !*** ./src/InfiniLore.Server/TsLib/SecureStorage.ts ***!
  \******************************************************/
/***/ (function(__unused_webpack_module, exports) {


var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", ({ value: true }));
exports.SecureStorageService = void 0;
class SecureStorageService {
    _openDatabaseAsync() {
        return __awaiter(this, void 0, void 0, function* () {
            return new Promise((resolve, reject) => {
                const request = indexedDB.open(SecureStorageService.DB_NAME, SecureStorageService.DB_VERSION);
                request.onupgradeneeded = (event) => {
                    const db = event.target.result;
                    console.log("Upgrading database (v3). Existing object stores:", db.objectStoreNames);
                    if (!db.objectStoreNames.contains(SecureStorageService.STORE_NAME)) {
                        console.log("Creating object store 'tokens'");
                        db.createObjectStore(SecureStorageService.STORE_NAME, { keyPath: "id" });
                    }
                };
                request.onsuccess = (event) => {
                    const db = event.target.result;
                    console.log("Database opened successfully. Object stores:", db.objectStoreNames);
                    if (!db.objectStoreNames.contains(SecureStorageService.STORE_NAME)) {
                        console.warn("Object store 'tokens' not found even after opening DB.");
                    }
                    resolve(db);
                };
                request.onerror = (event) => {
                    reject("Failed to open IndexedDB: " + event.target.error);
                };
            });
        });
    }
    _executeTransactionAsync(storeName, mode, executeCallback) {
        return __awaiter(this, void 0, void 0, function* () {
            const db = yield this._openDatabaseAsync();
            if (!db.objectStoreNames.contains(storeName)) {
                console.error(`Object store '${storeName}' not found.`);
                return Promise.reject(`Object store '${storeName}' does not exist.`);
            }
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
        });
    }
    saveTokenAsync(key, value, expiresAt) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readwrite", (store) => store.put({ id: key, value: value, expiresAt: expiresAt }));
        });
    }
    getTokenAsync(key) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readonly", (store) => store.get(key));
        });
    }
    removeTokenAsync(key) {
        return __awaiter(this, void 0, void 0, function* () {
            return yield this._executeTransactionAsync(SecureStorageService.STORE_NAME, "readwrite", (store) => store.delete(key));
        });
    }
}
exports.SecureStorageService = SecureStorageService;
SecureStorageService.DB_NAME = "JwtStorage";
SecureStorageService.DB_VERSION = 3;
SecureStorageService.STORE_NAME = "tokens";


/***/ })

/******/ 	});
/************************************************************************/
/******/ 	// The module cache
/******/ 	var __webpack_module_cache__ = {};
/******/ 	
/******/ 	// The require function
/******/ 	function __webpack_require__(moduleId) {
/******/ 		// Check if module is in cache
/******/ 		var cachedModule = __webpack_module_cache__[moduleId];
/******/ 		if (cachedModule !== undefined) {
/******/ 			return cachedModule.exports;
/******/ 		}
/******/ 		// Create a new module (and put it into the cache)
/******/ 		var module = __webpack_module_cache__[moduleId] = {
/******/ 			// no module.id needed
/******/ 			// no module.loaded needed
/******/ 			exports: {}
/******/ 		};
/******/ 	
/******/ 		// Execute the module function
/******/ 		__webpack_modules__[moduleId].call(module.exports, module, module.exports, __webpack_require__);
/******/ 	
/******/ 		// Return the exports of the module
/******/ 		return module.exports;
/******/ 	}
/******/ 	
/************************************************************************/
var __webpack_exports__ = {};
// This entry needs to be wrapped in an IIFE because it needs to be isolated against other modules in the chunk.
(() => {
var exports = __webpack_exports__;
/*!**********************************************!*\
  !*** ./src/InfiniLore.Server/TsLib/index.ts ***!
  \**********************************************/

Object.defineProperty(exports, "__esModule", ({ value: true }));
const SecureStorage_1 = __webpack_require__(/*! ./SecureStorage */ "./src/InfiniLore.Server/TsLib/SecureStorage.ts");
window.secureStorage = new SecureStorage_1.SecureStorageService();

})();

/******/ })()
;
//# sourceMappingURL=data:application/json;charset=utf-8;base64,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QUNBOztVQUVBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBOztVQUVBO1VBQ0E7O1VBRUE7VUFDQTtVQUNBOzs7Ozs7Ozs7Ozs7QUNuQkEscUhBQXFEO0FBS3JELE1BQU0sQ0FBQyxhQUFhLEdBQUcsSUFBSSxvQ0FBb0IsRUFBRSxDQUFDIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vaW5maW5pbG9yZS8uL3NyYy9JbmZpbmlMb3JlLlNlcnZlci9Uc0xpYi9TZWN1cmVTdG9yYWdlLnRzIiwid2VicGFjazovL2luZmluaWxvcmUvd2VicGFjay9ib290c3RyYXAiLCJ3ZWJwYWNrOi8vaW5maW5pbG9yZS8uL3NyYy9JbmZpbmlMb3JlLlNlcnZlci9Uc0xpYi9pbmRleC50cyJdLCJzb3VyY2VzQ29udGVudCI6WyIvLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gSW1wb3J0c1xyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuaW1wb3J0IHsgU2VjdXJlU3RvcmFnZSwgVG9rZW5EYXRhIH0gZnJvbSBcIi4vQ29udHJhY3RzXCI7XHJcblxyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gQ29kZVxyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuZXhwb3J0IGNsYXNzIFNlY3VyZVN0b3JhZ2VTZXJ2aWNlIGltcGxlbWVudHMgU2VjdXJlU3RvcmFnZSB7XHJcbiAgICBwcml2YXRlIHN0YXRpYyByZWFkb25seSBEQl9OQU1FID0gXCJKd3RTdG9yYWdlXCI7XHJcbiAgICBwcml2YXRlIHN0YXRpYyByZWFkb25seSBEQl9WRVJTSU9OID0gMztcclxuICAgIHByaXZhdGUgc3RhdGljIHJlYWRvbmx5IFNUT1JFX05BTUUgPSBcInRva2Vuc1wiO1xyXG5cclxuICAgIHByaXZhdGUgYXN5bmMgX29wZW5EYXRhYmFzZUFzeW5jKCk6IFByb21pc2U8SURCRGF0YWJhc2U+IHtcclxuICAgICAgICByZXR1cm4gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICAgICAgICBjb25zdCByZXF1ZXN0OiBJREJPcGVuREJSZXF1ZXN0ID0gaW5kZXhlZERCLm9wZW4oU2VjdXJlU3RvcmFnZVNlcnZpY2UuREJfTkFNRSwgU2VjdXJlU3RvcmFnZVNlcnZpY2UuREJfVkVSU0lPTik7XHJcblxyXG4gICAgICAgICAgICByZXF1ZXN0Lm9udXBncmFkZW5lZWRlZCA9IChldmVudDogSURCVmVyc2lvbkNoYW5nZUV2ZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICBjb25zdCBkYjogSURCRGF0YWJhc2UgPSAoZXZlbnQudGFyZ2V0IGFzIElEQk9wZW5EQlJlcXVlc3QpLnJlc3VsdDtcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiVXBncmFkaW5nIGRhdGFiYXNlICh2MykuIEV4aXN0aW5nIG9iamVjdCBzdG9yZXM6XCIsIGRiLm9iamVjdFN0b3JlTmFtZXMpO1xyXG4gICAgICAgICAgICAgICAgaWYgKCFkYi5vYmplY3RTdG9yZU5hbWVzLmNvbnRhaW5zKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS5sb2coXCJDcmVhdGluZyBvYmplY3Qgc3RvcmUgJ3Rva2VucydcIik7XHJcbiAgICAgICAgICAgICAgICAgICAgZGIuY3JlYXRlT2JqZWN0U3RvcmUoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSwgeyBrZXlQYXRoOiBcImlkXCIgfSk7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICByZXF1ZXN0Lm9uc3VjY2VzcyA9IChldmVudDogRXZlbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIGNvbnN0IGRiOiBJREJEYXRhYmFzZSA9IChldmVudC50YXJnZXQgYXMgSURCT3BlbkRCUmVxdWVzdCkucmVzdWx0O1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS5sb2coXCJEYXRhYmFzZSBvcGVuZWQgc3VjY2Vzc2Z1bGx5LiBPYmplY3Qgc3RvcmVzOlwiLCBkYi5vYmplY3RTdG9yZU5hbWVzKTtcclxuICAgICAgICAgICAgICAgIGlmICghZGIub2JqZWN0U3RvcmVOYW1lcy5jb250YWlucyhTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIGNvbnNvbGUud2FybihcIk9iamVjdCBzdG9yZSAndG9rZW5zJyBub3QgZm91bmQgZXZlbiBhZnRlciBvcGVuaW5nIERCLlwiKTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgICAgIHJlc29sdmUoZGIpO1xyXG4gICAgICAgICAgICB9O1xyXG5cclxuICAgICAgICAgICAgcmVxdWVzdC5vbmVycm9yID0gKGV2ZW50OiBFdmVudCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgcmVqZWN0KFwiRmFpbGVkIHRvIG9wZW4gSW5kZXhlZERCOiBcIiArIChldmVudC50YXJnZXQgYXMgSURCT3BlbkRCUmVxdWVzdCkuZXJyb3IpO1xyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHByaXZhdGUgYXN5bmMgX2V4ZWN1dGVUcmFuc2FjdGlvbkFzeW5jKFxyXG4gICAgICAgIHN0b3JlTmFtZTogc3RyaW5nLFxyXG4gICAgICAgIG1vZGU6IElEQlRyYW5zYWN0aW9uTW9kZSxcclxuICAgICAgICBleGVjdXRlQ2FsbGJhY2s6IChzdG9yZTogSURCT2JqZWN0U3RvcmUpID0+IElEQlJlcXVlc3RcclxuICAgICk6IFByb21pc2U8YW55PiB7XHJcbiAgICAgICAgY29uc3QgZGIgPSBhd2FpdCB0aGlzLl9vcGVuRGF0YWJhc2VBc3luYygpO1xyXG5cclxuICAgICAgICBpZiAoIWRiLm9iamVjdFN0b3JlTmFtZXMuY29udGFpbnMoc3RvcmVOYW1lKSkge1xyXG4gICAgICAgICAgICBjb25zb2xlLmVycm9yKGBPYmplY3Qgc3RvcmUgJyR7c3RvcmVOYW1lfScgbm90IGZvdW5kLmApO1xyXG4gICAgICAgICAgICByZXR1cm4gUHJvbWlzZS5yZWplY3QoYE9iamVjdCBzdG9yZSAnJHtzdG9yZU5hbWV9JyBkb2VzIG5vdCBleGlzdC5gKTtcclxuICAgICAgICB9XHJcblxyXG4gICAgICAgIGNvbnN0IHRyYW5zYWN0aW9uOiBJREJUcmFuc2FjdGlvbiA9IGRiLnRyYW5zYWN0aW9uKFtzdG9yZU5hbWVdLCBtb2RlKTtcclxuICAgICAgICBjb25zdCBzdG9yZTogSURCT2JqZWN0U3RvcmUgPSB0cmFuc2FjdGlvbi5vYmplY3RTdG9yZShzdG9yZU5hbWUpO1xyXG5cclxuICAgICAgICByZXR1cm4gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICAgICAgICBjb25zdCByZXF1ZXN0OiBJREJSZXF1ZXN0ID0gZXhlY3V0ZUNhbGxiYWNrKHN0b3JlKTtcclxuXHJcbiAgICAgICAgICAgIHJlcXVlc3Qub25zdWNjZXNzID0gKGV2ZW50OiBFdmVudCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgcmVzb2x2ZSgoZXZlbnQudGFyZ2V0IGFzIElEQlJlcXVlc3QpLnJlc3VsdCB8fCBudWxsKTtcclxuICAgICAgICAgICAgfTtcclxuXHJcbiAgICAgICAgICAgIHJlcXVlc3Qub25lcnJvciA9IChldmVudDogRXZlbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIHJlamVjdChcIlRyYW5zYWN0aW9uIGZhaWxlZDogXCIgKyAoZXZlbnQudGFyZ2V0IGFzIElEQlJlcXVlc3QpLmVycm9yKTtcclxuICAgICAgICAgICAgfTtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBwdWJsaWMgYXN5bmMgc2F2ZVRva2VuQXN5bmMoa2V5OiBzdHJpbmcsIHZhbHVlOiBzdHJpbmcsIGV4cGlyZXNBdDogRGF0ZSk6IFByb21pc2U8YW55PiB7XHJcbiAgICAgICAgcmV0dXJuIGF3YWl0IHRoaXMuX2V4ZWN1dGVUcmFuc2FjdGlvbkFzeW5jKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUsIFwicmVhZHdyaXRlXCIsIChzdG9yZSkgPT5cclxuICAgICAgICAgICAgc3RvcmUucHV0KHsgaWQ6IGtleSwgdmFsdWU6IHZhbHVlLCBleHBpcmVzQXQ6IGV4cGlyZXNBdCB9KVxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgcHVibGljIGFzeW5jIGdldFRva2VuQXN5bmMoa2V5OiBzdHJpbmcpOiBQcm9taXNlPFRva2VuRGF0YSB8IG51bGw+IHtcclxuICAgICAgICByZXR1cm4gYXdhaXQgdGhpcy5fZXhlY3V0ZVRyYW5zYWN0aW9uQXN5bmMoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSwgXCJyZWFkb25seVwiLCAoc3RvcmUpID0+XHJcbiAgICAgICAgICAgIHN0b3JlLmdldChrZXkpXHJcbiAgICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICBwdWJsaWMgYXN5bmMgcmVtb3ZlVG9rZW5Bc3luYyhrZXk6IHN0cmluZyk6IFByb21pc2U8YW55PiB7XHJcbiAgICAgICAgcmV0dXJuIGF3YWl0IHRoaXMuX2V4ZWN1dGVUcmFuc2FjdGlvbkFzeW5jKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUsIFwicmVhZHdyaXRlXCIsIChzdG9yZSkgPT5cclxuICAgICAgICAgICAgc3RvcmUuZGVsZXRlKGtleSlcclxuICAgICAgICApO1xyXG4gICAgfVxyXG59XHJcbiIsIi8vIFRoZSBtb2R1bGUgY2FjaGVcbnZhciBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX18gPSB7fTtcblxuLy8gVGhlIHJlcXVpcmUgZnVuY3Rpb25cbmZ1bmN0aW9uIF9fd2VicGFja19yZXF1aXJlX18obW9kdWxlSWQpIHtcblx0Ly8gQ2hlY2sgaWYgbW9kdWxlIGlzIGluIGNhY2hlXG5cdHZhciBjYWNoZWRNb2R1bGUgPSBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX19bbW9kdWxlSWRdO1xuXHRpZiAoY2FjaGVkTW9kdWxlICE9PSB1bmRlZmluZWQpIHtcblx0XHRyZXR1cm4gY2FjaGVkTW9kdWxlLmV4cG9ydHM7XG5cdH1cblx0Ly8gQ3JlYXRlIGEgbmV3IG1vZHVsZSAoYW5kIHB1dCBpdCBpbnRvIHRoZSBjYWNoZSlcblx0dmFyIG1vZHVsZSA9IF9fd2VicGFja19tb2R1bGVfY2FjaGVfX1ttb2R1bGVJZF0gPSB7XG5cdFx0Ly8gbm8gbW9kdWxlLmlkIG5lZWRlZFxuXHRcdC8vIG5vIG1vZHVsZS5sb2FkZWQgbmVlZGVkXG5cdFx0ZXhwb3J0czoge31cblx0fTtcblxuXHQvLyBFeGVjdXRlIHRoZSBtb2R1bGUgZnVuY3Rpb25cblx0X193ZWJwYWNrX21vZHVsZXNfX1ttb2R1bGVJZF0uY2FsbChtb2R1bGUuZXhwb3J0cywgbW9kdWxlLCBtb2R1bGUuZXhwb3J0cywgX193ZWJwYWNrX3JlcXVpcmVfXyk7XG5cblx0Ly8gUmV0dXJuIHRoZSBleHBvcnRzIG9mIHRoZSBtb2R1bGVcblx0cmV0dXJuIG1vZHVsZS5leHBvcnRzO1xufVxuXG4iLCIvLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gSW1wb3J0c1xyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuaW1wb3J0IHtTZWN1cmVTdG9yYWdlU2VydmljZX0gZnJvbSBcIi4vU2VjdXJlU3RvcmFnZVwiO1xyXG5cclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vIENvZGVcclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbndpbmRvdy5zZWN1cmVTdG9yYWdlID0gbmV3IFNlY3VyZVN0b3JhZ2VTZXJ2aWNlKCk7Il0sIm5hbWVzIjpbXSwic291cmNlUm9vdCI6IiJ9