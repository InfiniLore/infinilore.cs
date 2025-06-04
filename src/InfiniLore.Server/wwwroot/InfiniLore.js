/******/ (() => { // webpackBootstrap
/******/ 	"use strict";
/******/ 	var __webpack_modules__ = ({

/***/ "./src/InfiniLore.Modules.Core.Shared/Services/JsRuntimeHelper/TsLib/SecureStorage.ts":
/*!********************************************************************************************!*\
  !*** ./src/InfiniLore.Modules.Core.Shared/Services/JsRuntimeHelper/TsLib/SecureStorage.ts ***!
  \********************************************************************************************/
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
/*!************************************************************************************!*\
  !*** ./src/InfiniLore.Modules.Core.Shared/Services/JsRuntimeHelper/TsLib/index.ts ***!
  \************************************************************************************/

Object.defineProperty(exports, "__esModule", ({ value: true }));
const SecureStorage_1 = __webpack_require__(/*! ./SecureStorage */ "./src/InfiniLore.Modules.Core.Shared/Services/JsRuntimeHelper/TsLib/SecureStorage.ts");
window.secureStorage = new SecureStorage_1.SecureStorageService();

})();

/******/ })()
;
//# sourceMappingURL=data:application/json;charset=utf-8;base64,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QUNBOztVQUVBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBO1VBQ0E7VUFDQTtVQUNBOztVQUVBO1VBQ0E7O1VBRUE7VUFDQTtVQUNBOzs7Ozs7Ozs7Ozs7QUNuQkEsMkpBQXFEO0FBS3JELE1BQU0sQ0FBQyxhQUFhLEdBQUcsSUFBSSxvQ0FBb0IsRUFBRSxDQUFDIiwic291cmNlcyI6WyJ3ZWJwYWNrOi8vaW5maW5pbG9yZS8uL3NyYy9JbmZpbmlMb3JlLk1vZHVsZXMuQ29yZS5TaGFyZWQvU2VydmljZXMvSnNSdW50aW1lSGVscGVyL1RzTGliL1NlY3VyZVN0b3JhZ2UudHMiLCJ3ZWJwYWNrOi8vaW5maW5pbG9yZS93ZWJwYWNrL2Jvb3RzdHJhcCIsIndlYnBhY2s6Ly9pbmZpbmlsb3JlLy4vc3JjL0luZmluaUxvcmUuTW9kdWxlcy5Db3JlLlNoYXJlZC9TZXJ2aWNlcy9Kc1J1bnRpbWVIZWxwZXIvVHNMaWIvaW5kZXgudHMiXSwic291cmNlc0NvbnRlbnQiOlsiLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vIEltcG9ydHNcclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbmltcG9ydCB7IFNlY3VyZVN0b3JhZ2UsIFRva2VuRGF0YSB9IGZyb20gXCIuL0NvbnRyYWN0c1wiO1xyXG5cclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vIENvZGVcclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbmV4cG9ydCBjbGFzcyBTZWN1cmVTdG9yYWdlU2VydmljZSBpbXBsZW1lbnRzIFNlY3VyZVN0b3JhZ2Uge1xyXG4gICAgcHJpdmF0ZSBzdGF0aWMgcmVhZG9ubHkgREJfTkFNRSA9IFwiSnd0U3RvcmFnZVwiO1xyXG4gICAgcHJpdmF0ZSBzdGF0aWMgcmVhZG9ubHkgREJfVkVSU0lPTiA9IDM7XHJcbiAgICBwcml2YXRlIHN0YXRpYyByZWFkb25seSBTVE9SRV9OQU1FID0gXCJ0b2tlbnNcIjtcclxuXHJcbiAgICBwcml2YXRlIGFzeW5jIF9vcGVuRGF0YWJhc2VBc3luYygpOiBQcm9taXNlPElEQkRhdGFiYXNlPiB7XHJcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9taXNlKChyZXNvbHZlLCByZWplY3QpID0+IHtcclxuICAgICAgICAgICAgY29uc3QgcmVxdWVzdDogSURCT3BlbkRCUmVxdWVzdCA9IGluZGV4ZWREQi5vcGVuKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLkRCX05BTUUsIFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLkRCX1ZFUlNJT04pO1xyXG5cclxuICAgICAgICAgICAgcmVxdWVzdC5vbnVwZ3JhZGVuZWVkZWQgPSAoZXZlbnQ6IElEQlZlcnNpb25DaGFuZ2VFdmVudCkgPT4ge1xyXG4gICAgICAgICAgICAgICAgY29uc3QgZGI6IElEQkRhdGFiYXNlID0gKGV2ZW50LnRhcmdldCBhcyBJREJPcGVuREJSZXF1ZXN0KS5yZXN1bHQ7XHJcbiAgICAgICAgICAgICAgICBjb25zb2xlLmxvZyhcIlVwZ3JhZGluZyBkYXRhYmFzZSAodjMpLiBFeGlzdGluZyBvYmplY3Qgc3RvcmVzOlwiLCBkYi5vYmplY3RTdG9yZU5hbWVzKTtcclxuICAgICAgICAgICAgICAgIGlmICghZGIub2JqZWN0U3RvcmVOYW1lcy5jb250YWlucyhTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FKSkge1xyXG4gICAgICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiQ3JlYXRpbmcgb2JqZWN0IHN0b3JlICd0b2tlbnMnXCIpO1xyXG4gICAgICAgICAgICAgICAgICAgIGRiLmNyZWF0ZU9iamVjdFN0b3JlKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUsIHsga2V5UGF0aDogXCJpZFwiIH0pO1xyXG4gICAgICAgICAgICAgICAgfVxyXG4gICAgICAgICAgICB9O1xyXG5cclxuICAgICAgICAgICAgcmVxdWVzdC5vbnN1Y2Nlc3MgPSAoZXZlbnQ6IEV2ZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICBjb25zdCBkYjogSURCRGF0YWJhc2UgPSAoZXZlbnQudGFyZ2V0IGFzIElEQk9wZW5EQlJlcXVlc3QpLnJlc3VsdDtcclxuICAgICAgICAgICAgICAgIGNvbnNvbGUubG9nKFwiRGF0YWJhc2Ugb3BlbmVkIHN1Y2Nlc3NmdWxseS4gT2JqZWN0IHN0b3JlczpcIiwgZGIub2JqZWN0U3RvcmVOYW1lcyk7XHJcbiAgICAgICAgICAgICAgICBpZiAoIWRiLm9iamVjdFN0b3JlTmFtZXMuY29udGFpbnMoU2VjdXJlU3RvcmFnZVNlcnZpY2UuU1RPUkVfTkFNRSkpIHtcclxuICAgICAgICAgICAgICAgICAgICBjb25zb2xlLndhcm4oXCJPYmplY3Qgc3RvcmUgJ3Rva2Vucycgbm90IGZvdW5kIGV2ZW4gYWZ0ZXIgb3BlbmluZyBEQi5cIik7XHJcbiAgICAgICAgICAgICAgICB9XHJcbiAgICAgICAgICAgICAgICByZXNvbHZlKGRiKTtcclxuICAgICAgICAgICAgfTtcclxuXHJcbiAgICAgICAgICAgIHJlcXVlc3Qub25lcnJvciA9IChldmVudDogRXZlbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIHJlamVjdChcIkZhaWxlZCB0byBvcGVuIEluZGV4ZWREQjogXCIgKyAoZXZlbnQudGFyZ2V0IGFzIElEQk9wZW5EQlJlcXVlc3QpLmVycm9yKTtcclxuICAgICAgICAgICAgfTtcclxuICAgICAgICB9KTtcclxuICAgIH1cclxuXHJcbiAgICBwcml2YXRlIGFzeW5jIF9leGVjdXRlVHJhbnNhY3Rpb25Bc3luYyhcclxuICAgICAgICBzdG9yZU5hbWU6IHN0cmluZyxcclxuICAgICAgICBtb2RlOiBJREJUcmFuc2FjdGlvbk1vZGUsXHJcbiAgICAgICAgZXhlY3V0ZUNhbGxiYWNrOiAoc3RvcmU6IElEQk9iamVjdFN0b3JlKSA9PiBJREJSZXF1ZXN0XHJcbiAgICApOiBQcm9taXNlPGFueT4ge1xyXG4gICAgICAgIGNvbnN0IGRiID0gYXdhaXQgdGhpcy5fb3BlbkRhdGFiYXNlQXN5bmMoKTtcclxuXHJcbiAgICAgICAgaWYgKCFkYi5vYmplY3RTdG9yZU5hbWVzLmNvbnRhaW5zKHN0b3JlTmFtZSkpIHtcclxuICAgICAgICAgICAgY29uc29sZS5lcnJvcihgT2JqZWN0IHN0b3JlICcke3N0b3JlTmFtZX0nIG5vdCBmb3VuZC5gKTtcclxuICAgICAgICAgICAgcmV0dXJuIFByb21pc2UucmVqZWN0KGBPYmplY3Qgc3RvcmUgJyR7c3RvcmVOYW1lfScgZG9lcyBub3QgZXhpc3QuYCk7XHJcbiAgICAgICAgfVxyXG5cclxuICAgICAgICBjb25zdCB0cmFuc2FjdGlvbjogSURCVHJhbnNhY3Rpb24gPSBkYi50cmFuc2FjdGlvbihbc3RvcmVOYW1lXSwgbW9kZSk7XHJcbiAgICAgICAgY29uc3Qgc3RvcmU6IElEQk9iamVjdFN0b3JlID0gdHJhbnNhY3Rpb24ub2JqZWN0U3RvcmUoc3RvcmVOYW1lKTtcclxuXHJcbiAgICAgICAgcmV0dXJuIG5ldyBQcm9taXNlKChyZXNvbHZlLCByZWplY3QpID0+IHtcclxuICAgICAgICAgICAgY29uc3QgcmVxdWVzdDogSURCUmVxdWVzdCA9IGV4ZWN1dGVDYWxsYmFjayhzdG9yZSk7XHJcblxyXG4gICAgICAgICAgICByZXF1ZXN0Lm9uc3VjY2VzcyA9IChldmVudDogRXZlbnQpID0+IHtcclxuICAgICAgICAgICAgICAgIHJlc29sdmUoKGV2ZW50LnRhcmdldCBhcyBJREJSZXF1ZXN0KS5yZXN1bHQgfHwgbnVsbCk7XHJcbiAgICAgICAgICAgIH07XHJcblxyXG4gICAgICAgICAgICByZXF1ZXN0Lm9uZXJyb3IgPSAoZXZlbnQ6IEV2ZW50KSA9PiB7XHJcbiAgICAgICAgICAgICAgICByZWplY3QoXCJUcmFuc2FjdGlvbiBmYWlsZWQ6IFwiICsgKGV2ZW50LnRhcmdldCBhcyBJREJSZXF1ZXN0KS5lcnJvcik7XHJcbiAgICAgICAgICAgIH07XHJcbiAgICAgICAgfSk7XHJcbiAgICB9XHJcblxyXG4gICAgcHVibGljIGFzeW5jIHNhdmVUb2tlbkFzeW5jKGtleTogc3RyaW5nLCB2YWx1ZTogc3RyaW5nLCBleHBpcmVzQXQ6IERhdGUpOiBQcm9taXNlPGFueT4ge1xyXG4gICAgICAgIHJldHVybiBhd2FpdCB0aGlzLl9leGVjdXRlVHJhbnNhY3Rpb25Bc3luYyhTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FLCBcInJlYWR3cml0ZVwiLCAoc3RvcmUpID0+XHJcbiAgICAgICAgICAgIHN0b3JlLnB1dCh7IGlkOiBrZXksIHZhbHVlOiB2YWx1ZSwgZXhwaXJlc0F0OiBleHBpcmVzQXQgfSlcclxuICAgICAgICApO1xyXG4gICAgfVxyXG5cclxuICAgIHB1YmxpYyBhc3luYyBnZXRUb2tlbkFzeW5jKGtleTogc3RyaW5nKTogUHJvbWlzZTxUb2tlbkRhdGEgfCBudWxsPiB7XHJcbiAgICAgICAgcmV0dXJuIGF3YWl0IHRoaXMuX2V4ZWN1dGVUcmFuc2FjdGlvbkFzeW5jKFNlY3VyZVN0b3JhZ2VTZXJ2aWNlLlNUT1JFX05BTUUsIFwicmVhZG9ubHlcIiwgKHN0b3JlKSA9PlxyXG4gICAgICAgICAgICBzdG9yZS5nZXQoa2V5KVxyXG4gICAgICAgICk7XHJcbiAgICB9XHJcblxyXG4gICAgcHVibGljIGFzeW5jIHJlbW92ZVRva2VuQXN5bmMoa2V5OiBzdHJpbmcpOiBQcm9taXNlPGFueT4ge1xyXG4gICAgICAgIHJldHVybiBhd2FpdCB0aGlzLl9leGVjdXRlVHJhbnNhY3Rpb25Bc3luYyhTZWN1cmVTdG9yYWdlU2VydmljZS5TVE9SRV9OQU1FLCBcInJlYWR3cml0ZVwiLCAoc3RvcmUpID0+XHJcbiAgICAgICAgICAgIHN0b3JlLmRlbGV0ZShrZXkpXHJcbiAgICAgICAgKTtcclxuICAgIH1cclxufVxyXG4iLCIvLyBUaGUgbW9kdWxlIGNhY2hlXG52YXIgX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fID0ge307XG5cbi8vIFRoZSByZXF1aXJlIGZ1bmN0aW9uXG5mdW5jdGlvbiBfX3dlYnBhY2tfcmVxdWlyZV9fKG1vZHVsZUlkKSB7XG5cdC8vIENoZWNrIGlmIG1vZHVsZSBpcyBpbiBjYWNoZVxuXHR2YXIgY2FjaGVkTW9kdWxlID0gX193ZWJwYWNrX21vZHVsZV9jYWNoZV9fW21vZHVsZUlkXTtcblx0aWYgKGNhY2hlZE1vZHVsZSAhPT0gdW5kZWZpbmVkKSB7XG5cdFx0cmV0dXJuIGNhY2hlZE1vZHVsZS5leHBvcnRzO1xuXHR9XG5cdC8vIENyZWF0ZSBhIG5ldyBtb2R1bGUgKGFuZCBwdXQgaXQgaW50byB0aGUgY2FjaGUpXG5cdHZhciBtb2R1bGUgPSBfX3dlYnBhY2tfbW9kdWxlX2NhY2hlX19bbW9kdWxlSWRdID0ge1xuXHRcdC8vIG5vIG1vZHVsZS5pZCBuZWVkZWRcblx0XHQvLyBubyBtb2R1bGUubG9hZGVkIG5lZWRlZFxuXHRcdGV4cG9ydHM6IHt9XG5cdH07XG5cblx0Ly8gRXhlY3V0ZSB0aGUgbW9kdWxlIGZ1bmN0aW9uXG5cdF9fd2VicGFja19tb2R1bGVzX19bbW9kdWxlSWRdLmNhbGwobW9kdWxlLmV4cG9ydHMsIG1vZHVsZSwgbW9kdWxlLmV4cG9ydHMsIF9fd2VicGFja19yZXF1aXJlX18pO1xuXG5cdC8vIFJldHVybiB0aGUgZXhwb3J0cyBvZiB0aGUgbW9kdWxlXG5cdHJldHVybiBtb2R1bGUuZXhwb3J0cztcbn1cblxuIiwiLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbi8vIEltcG9ydHNcclxuLy8gLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tXHJcbmltcG9ydCB7U2VjdXJlU3RvcmFnZVNlcnZpY2V9IGZyb20gXCIuL1NlY3VyZVN0b3JhZ2VcIjtcclxuXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG4vLyBDb2RlXHJcbi8vIC0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLS0tLVxyXG53aW5kb3cuc2VjdXJlU3RvcmFnZSA9IG5ldyBTZWN1cmVTdG9yYWdlU2VydmljZSgpOyJdLCJuYW1lcyI6W10sInNvdXJjZVJvb3QiOiIifQ==