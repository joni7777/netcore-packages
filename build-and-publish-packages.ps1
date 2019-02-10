function Build-Packages {
    $projects = Get-ChildItem "*.csproj" -Recurse
    $sln = Get-Item *.sln
    dotnet build $sln.FullName -c Release
    
    foreach($project in $projects) {
        dotnet pack $project.FullName --no-build -c Release -o ../artifacts
    }
}

function Publish-Packages {
    Build-Packages
    
    $packages = Get-ChildItem "./artifacts/*.nupkg"
    
    foreach($package in $packages) {
        dotnet nuget push $package.FullName
    }
}

Publish-Packages