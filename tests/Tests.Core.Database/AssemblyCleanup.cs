// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using TUnit.Core.Logging;

namespace Tests.Core.Database;

// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public static class AssemblyCleanup {
    [After(TestSession)]
    public static void CleanupTestDb(TestSessionContext sessionContext) {
        string currentDirectory = Directory.GetCurrentDirectory();
        
        string[] testDbFiles = Directory.GetFiles(currentDirectory, "test_*.db", SearchOption.AllDirectories);
        
        foreach (string dbFile in testDbFiles) {
            try {
                string journalFile = dbFile + "-journal";
                if (File.Exists(journalFile)) {
                    File.Delete(journalFile);
                }
                
                File.Delete(dbFile);
            }
            catch (Exception ex) {
                sessionContext.GetDefaultLogger().LogWarning($"Failed to delete {dbFile}: {ex.Message}");
            }
        }
    }
}