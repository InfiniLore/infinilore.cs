# -----------------------------------------------------------------------------------------------------------------
# Tweakable variables
# -----------------------------------------------------------------------------------------------------------------
$Language = "CSharp"
$NameSpace = "InfiniLore.KiotaApiClient"
$OutputFolder = "./src/client/InfiniLore.KiotaApiClient/"
$ClassName = "InfiniLoreApiClient"
$OpenApiFile = "https://localhost:7059/swagger/v1/swagger.json"

$CsprojPath = "./src/lib/InfiniLore.KiotaApiClient/InfiniLore.KiotaApiClient.csproj"
$TempCsprojPath = "./.temp/InfiniLore.KiotaApiClient.csproj"

# -----------------------------------------------------------------------------------------------------------------
# Code
# -----------------------------------------------------------------------------------------------------------------
function Copy-Csproj {
    if (Test-Path -Path $CsprojPath) {
        Copy-Item -Path $CsprojPath -Destination $TempCsprojPath
    }
}

function Restore-Csproj {
    cd "../../../"
    if (-not (Test-Path -Path $CsprojPath) -and (Test-Path -Path $TempCsprojPath)) {
        Move-Item -Path $TempCsprojPath -Destination $CsprojPath
    }
}

function main {
    echo "Copying .csproj file to temporary location..."
    Copy-Csproj

    echo "Generating OpenApi client with Kiota..."
    cd $OutputFolder
    kiota generate `
        --openapi $OpenApiFile `
        --language $Language `
        --namespace-name $NameSpace `
        --backing-store false `
        --class-name $ClassName `
        --output ./ `
        --exclude-backward-compatible `
        --clean-output `
        --clear-cache

    echo "Restoring .csproj file if it no longer exists..."
    Restore-Csproj
    echo "Finished"
}

main