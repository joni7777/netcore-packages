param(
    [parameter(Mandatory=$true, HelpMessage="Enter Nuget API key and source values")]
    [String]$nugetApiKey, [String]$nugetApiSource)

function Clean-Artifacts {
    rm ./artifacts/*.nupkg 
}

function Build-Packages {
    Clean-Artifacts
    
    $projects = Get-ChildItem "*.csproj" -Recurse
    $sln = Get-Item *.sln
    dotnet build $sln.FullName -c Release
    
    foreach($project in $projects) {
        dotnet pack $project.FullName --no-build -c Release -o ../artifacts
    }
}

function Publish-Packages {
    $packages = Get-ChildItem "./artifacts/*.nupkg"
    
    foreach($package in $packages) {
        dotnet nuget push $package.FullName -s $nugetApiSource -k $nugetApiKey
    }
}

Write-Host "Starting to build packages"
Build-Packages
Write-Host "Finished building packages"

if(!$nugetApiKey -Or !$nugetApiSource) {
    Write-Host "Not uploading packages, nugetApiKey or nugetApiSource is null and both must be provided to upload the packages"
} else {
    Write-Host "Starting to publish packages"
    Publish-Packages
    Write-Host "Finished publish packages"
}