/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/InfiniLore.Shared.JsInterop/TsLib/SecureStorage.ts":
/*!****************************************************************!*\
  !*** ./src/InfiniLore.Shared.JsInterop/TsLib/SecureStorage.ts ***!
  \****************************************************************/
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
/*!********************************************************!*\
  !*** ./src/InfiniLore.Shared.JsInterop/TsLib/index.ts ***!
  \********************************************************/

Object.defineProperty(exports, "__esModule", ({ value: true }));
const SecureStorage_1 = __webpack_require__(/*! ./SecureStorage */ "./src/InfiniLore.Shared.JsInterop/TsLib/SecureStorage.ts");
window.secureStorage = new SecureStorage_1.SecureStorageService();

})();

/******/ })()
;
//# sourceMappingURL=data:application/json;charset=utf-8;base64,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QUNBOztVQUVBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBOztVQUVBO1VBQ0E7O1VBRUE7VUFDQTtVQUNBOzs7Ozs7Ozs7Ozs7QUNuQkEsK0hBQXFEO0FBS3JELE1BQU0sQ0FBQyxhQUFhLEdBQUcsSUFBSSxvQ0FBb0IsRUFBRSxDQUFDIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vaW5maW5pbG9yZS8uL3NyYy9JbmZpbmlMb3JlLlNoYXJlZC5Kc0ludGVyb3AvVHNMaWIvU2VjdXJlU3RvcmFnZS50cyIsIndlYnBhY2s6Ly9pbmZpbmlsb3JlL3dlYnBhY2svYm9vdHN0cmFwIiwid2VicGFjazovL2luZmluaWxvcmUvLi9zcmMvSW5maW5pTG9yZS5TaGFyZWQuSnNJbnRlcm9wL1RzTGliL2luZGV4LnRzIl0sInNvdXJjZXNDb250ZW50IjpbIi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyBJbXBvcnRzXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG5pbXBvcnQgeyBTZWN1cmVTdG9yYWdlLCBUb2tlbkRhdGEgfSBmcm9tIFwiLi9Db250cmFjdHNcIjtcclxuXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyBDb2RlXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG5leHBvcnQgY2xhc3MgU2VjdXJlU3RvcmFnZVNlcnZpY2UgaW1wbGVtZW50cyBTZWN1cmVTdG9yYWdlIHtcclxuICAgIHByaXZhdGUgc3RhdGljIHJlYWRvbmx5IERCX05BTUUgPSBcIkp3dFN0b3JhZ2VcIjtcclxuICAgIHByaXZhdGUgc3RhdGljIHJlYWRvbmx5IERCX1ZFUlNJT04gPSAzO1xyXG4gICAgcHJpdmF0ZSBzdGF0aWMgcmVhZG9ubHkgU1RPUkVfTkFNRSA9IFwidG9rZW5zXCI7XHJcblxyXG4gICAgcHJpdmF0ZSBhc3luYyBfb3BlbkRhdGFiYXNlQXN5bmMoKTogUHJvbWlzZTxJREJEYXRhYmFzZT4ge1xyXG4gICAgICAgIHJldHVybiBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XHJcbiAgICAgICAgICAgIGNvbnN0IHJlcXVlc3Q6IElEQk9wZW5EQlJlcXVlc3QgPSBpbmRleGVkREIub3BlbihTZWN1cmVTdG9yYWdlU2VydmljZS5EQl9OQU1FLCBTZWN1cmVTdG9yYWdlU2VydmljZS5EQl9WRVJTSU9OKTtcclxuXHJcbiAgICAgICAgICAgIHJlcXVlc3Qub251cGdyYWRlbmVlZGVkID0gKGV2ZW50OiBJREJWZXJzaW9uQ2hhbmdlRXZlbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIGNvbnN0IGRiOiBJREJEYXRhYmFzZSA9IChldmVudC50YXJnZXQgYXMgSURCT3BlbkRCUmVxdWVzdCkucmVzdWx0O1xyXG4gICAgICAgICAgICAgICAgY29uc29sZS5sb2coXCJVcGdyYWRpbmcgZGF0YWJhc2UgKHYzKS4gRXhpc3Rpbmcgb2JqZWN0IHN0b3JlczpcIiwgZGIub2JqZWN0U3RvcmVOYW1lcyk7XHJcbiAgICAgICAgICAgICAgICBpZiAoIWRiLm9iamVjdFN0b3JlTmFtZXMuY29udGFpbnMoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSkpIHtcclxuICAgICAgICAgICAgICAgICAgICBjb25zb2xlLmxvZyhcIkNyZWF0aW5nIG9iamVjdCBzdG9yZSAndG9rZW5zJ1wiKTtcclxuICAgICAgICAgICAgICAgICAgICBkYi5jcmVhdGVPYmplY3RTdG9yZShTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FLCB7IGtleVBhdGg6IFwiaWRcIiB9KTtcclxuICAgICAgICAgICAgICAgIH1cclxuICAgICAgICAgICAgfTtcclxuXHJcbiAgICAgICAgICAgIHJlcXVlc3Qub25zdWNjZXNzID0gKGV2ZW50OiBFdmVudCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgY29uc3QgZGI6IElEQkRhdGFiYXNlID0gKGV2ZW50LnRhcmdldCBhcyBJREJPcGVuREJSZXF1ZXN0KS5yZXN1bHQ7XHJcbiAgICAgICAgICAgICAgICBjb25zb2xlLmxvZyhcIkRhdGFiYXNlIG9wZW5lZCBzdWNjZXNzZnVsbHkuIE9iamVjdCBzdG9yZXM6XCIsIGRiLm9iamVjdFN0b3JlTmFtZXMpO1xyXG4gICAgICAgICAgICAgICAgaWYgKCFkYi5vYmplY3RTdG9yZU5hbWVzLmNvbnRhaW5zKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUpKSB7XHJcbiAgICAgICAgICAgICAgICAgICAgY29uc29sZS53YXJuKFwiT2JqZWN0IHN0b3JlICd0b2tlbnMnIG5vdCBmb3VuZCBldmVuIGFmdGVyIG9wZW5pbmcgREIuXCIpO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICAgICAgcmVzb2x2ZShkYik7XHJcbiAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICByZXF1ZXN0Lm9uZXJyb3IgPSAoZXZlbnQ6IEV2ZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICByZWplY3QoXCJGYWlsZWQgdG8gb3BlbiBJbmRleGVkREI6IFwiICsgKGV2ZW50LnRhcmdldCBhcyBJREJPcGVuREJSZXF1ZXN0KS5lcnJvcik7XHJcbiAgICAgICAgICAgIH07XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgcHJpdmF0ZSBhc3luYyBfZXhlY3V0ZVRyYW5zYWN0aW9uQXN5bmMoXHJcbiAgICAgICAgc3RvcmVOYW1lOiBzdHJpbmcsXHJcbiAgICAgICAgbW9kZTogSURCVHJhbnNhY3Rpb25Nb2RlLFxyXG4gICAgICAgIGV4ZWN1dGVDYWxsYmFjazogKHN0b3JlOiBJREJPYmplY3RTdG9yZSkgPT4gSURCUmVxdWVzdFxyXG4gICAgKTogUHJvbWlzZTxhbnk+IHtcclxuICAgICAgICBjb25zdCBkYiA9IGF3YWl0IHRoaXMuX29wZW5EYXRhYmFzZUFzeW5jKCk7XHJcblxyXG4gICAgICAgIGlmICghZGIub2JqZWN0U3RvcmVOYW1lcy5jb250YWlucyhzdG9yZU5hbWUpKSB7XHJcbiAgICAgICAgICAgIGNvbnNvbGUuZXJyb3IoYE9iamVjdCBzdG9yZSAnJHtzdG9yZU5hbWV9JyBub3QgZm91bmQuYCk7XHJcbiAgICAgICAgICAgIHJldHVybiBQcm9taXNlLnJlamVjdChgT2JqZWN0IHN0b3JlICcke3N0b3JlTmFtZX0nIGRvZXMgbm90IGV4aXN0LmApO1xyXG4gICAgICAgIH1cclxuXHJcbiAgICAgICAgY29uc3QgdHJhbnNhY3Rpb246IElEQlRyYW5zYWN0aW9uID0gZGIudHJhbnNhY3Rpb24oW3N0b3JlTmFtZV0sIG1vZGUpO1xyXG4gICAgICAgIGNvbnN0IHN0b3JlOiBJREJPYmplY3RTdG9yZSA9IHRyYW5zYWN0aW9uLm9iamVjdFN0b3JlKHN0b3JlTmFtZSk7XHJcblxyXG4gICAgICAgIHJldHVybiBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XHJcbiAgICAgICAgICAgIGNvbnN0IHJlcXVlc3Q6IElEQlJlcXVlc3QgPSBleGVjdXRlQ2FsbGJhY2soc3RvcmUpO1xyXG5cclxuICAgICAgICAgICAgcmVxdWVzdC5vbnN1Y2Nlc3MgPSAoZXZlbnQ6IEV2ZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICByZXNvbHZlKChldmVudC50YXJnZXQgYXMgSURCUmVxdWVzdCkucmVzdWx0IHx8IG51bGwpO1xyXG4gICAgICAgICAgICB9O1xyXG5cclxuICAgICAgICAgICAgcmVxdWVzdC5vbmVycm9yID0gKGV2ZW50OiBFdmVudCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgcmVqZWN0KFwiVHJhbnNhY3Rpb24gZmFpbGVkOiBcIiArIChldmVudC50YXJnZXQgYXMgSURCUmVxdWVzdCkuZXJyb3IpO1xyXG4gICAgICAgICAgICB9O1xyXG4gICAgICAgIH0pO1xyXG4gICAgfVxyXG5cclxuICAgIHB1YmxpYyBhc3luYyBzYXZlVG9rZW5Bc3luYyhrZXk6IHN0cmluZywgdmFsdWU6IHN0cmluZywgZXhwaXJlc0F0OiBEYXRlKTogUHJvbWlzZTxhbnk+IHtcclxuICAgICAgICByZXR1cm4gYXdhaXQgdGhpcy5fZXhlY3V0ZVRyYW5zYWN0aW9uQXN5bmMoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSwgXCJyZWFkd3JpdGVcIiwgKHN0b3JlKSA9PlxyXG4gICAgICAgICAgICBzdG9yZS5wdXQoeyBpZDoga2V5LCB2YWx1ZTogdmFsdWUsIGV4cGlyZXNBdDogZXhwaXJlc0F0IH0pXHJcbiAgICAgICAgKTtcclxuICAgIH1cclxuXHJcbiAgICBwdWJsaWMgYXN5bmMgZ2V0VG9rZW5Bc3luYyhrZXk6IHN0cmluZyk6IFByb21pc2U8VG9rZW5EYXRhIHwgbnVsbD4ge1xyXG4gICAgICAgIHJldHVybiBhd2FpdCB0aGlzLl9leGVjdXRlVHJhbnNhY3Rpb25Bc3luYyhTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FLCBcInJlYWRvbmx5XCIsIChzdG9yZSkgPT5cclxuICAgICAgICAgICAgc3RvcmUuZ2V0KGtleSlcclxuICAgICAgICApO1xyXG4gICAgfVxyXG5cclxuICAgIHB1YmxpYyBhc3luYyByZW1vdmVUb2tlbkFzeW5jKGtleTogc3RyaW5nKTogUHJvbWlzZTxhbnk+IHtcclxuICAgICAgICByZXR1cm4gYXdhaXQgdGhpcy5fZXhlY3V0ZVRyYW5zYWN0aW9uQXN5bmMoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSwgXCJyZWFkd3JpdGVcIiwgKHN0b3JlKSA9PlxyXG4gICAgICAgICAgICBzdG9yZS5kZWxldGUoa2V5KVxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcbn1cclxuIiwiLy8gVGhlIG1vZHVsZSBjYWNoZVxudmFyIF9fd2VicGFja19tb2R1bGVfY2FjaGVfXyA9IHt9O1xuXG4vLyBUaGUgcmVxdWlyZSBmdW5jdGlvblxuZnVuY3Rpb24gX193ZWJwYWNrX3JlcXVpcmVfXyhtb2R1bGVJZCkge1xuXHQvLyBDaGVjayBpZiBtb2R1bGUgaXMgaW4gY2FjaGVcblx0dmFyIGNhY2hlZE1vZHVsZSA9IF9fd2VicGFja19tb2R1bGVfY2FjaGVfX1ttb2R1bGVJZF07XG5cdGlmIChjYWNoZWRNb2R1bGUgIT09IHVuZGVmaW5lZCkge1xuXHRcdHJldHVybiBjYWNoZWRNb2R1bGUuZXhwb3J0cztcblx0fVxuXHQvLyBDcmVhdGUgYSBuZXcgbW9kdWxlIChhbmQgcHV0IGl0IGludG8gdGhlIGNhY2hlKVxuXHR2YXIgbW9kdWxlID0gX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fW21vZHVsZUlkXSA9IHtcblx0XHQvLyBubyBtb2R1bGUuaWQgbmVlZGVkXG5cdFx0Ly8gbm8gbW9kdWxlLmxvYWRlZCBuZWVkZWRcblx0XHRleHBvcnRzOiB7fVxuXHR9O1xuXG5cdC8vIEV4ZWN1dGUgdGhlIG1vZHVsZSBmdW5jdGlvblxuXHRfX3dlYnBhY2tfbW9kdWxlc19fW21vZHVsZUlkXS5jYWxsKG1vZHVsZS5leHBvcnRzLCBtb2R1bGUsIG1vZHVsZS5leHBvcnRzLCBfX3dlYnBhY2tfcmVxdWlyZV9fKTtcblxuXHQvLyBSZXR1cm4gdGhlIGV4cG9ydHMgb2YgdGhlIG1vZHVsZVxuXHRyZXR1cm4gbW9kdWxlLmV4cG9ydHM7XG59XG5cbiIsIi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyBJbXBvcnRzXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG5pbXBvcnQge1NlY3VyZVN0b3JhZ2VTZXJ2aWNlfSBmcm9tIFwiLi9TZWN1cmVTdG9yYWdlXCI7XHJcblxyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxuLy8gQ29kZVxyXG4vLyAtLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS1cclxud2luZG93LnNlY3VyZVN0b3JhZ2UgPSBuZXcgU2VjdXJlU3RvcmFnZVNlcnZpY2UoKTsiXSwibmFtZXMiOltdLCJzb3VyY2VSb290IjoiIn0=