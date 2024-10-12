Set-Location src/server/InfiniLore.Server

$NewMigrationName = Read-Host -Prompt "Enter the name of the new migration"

dotnet ef migrations add $NewMigrationName `
    --project ../InfiniLore.Server.Data.MigrationsSqlite `
    --startup-project ../InfiniLore.Server `
    --context InfiniLoreDbContext