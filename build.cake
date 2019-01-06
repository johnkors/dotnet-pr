var target = Argument("target", "Pack");
var project = "PR";
var nugetpackageId = "dotnet-pr";
var version = GetBuildVersion("1.5.0");
var outputDir = $"./builds/{project}";

Task("Pack")
    .Does(() => {
        var publishSettings = new DotNetCorePackSettings{
            OutputDirectory = outputDir,
            Configuration = "Release"
        };

        publishSettings.MSBuildSettings = new DotNetCoreMSBuildSettings()
                .WithProperty("Version", new[] { version });

        DotNetCorePack($"./source/{project}/{project}.csproj", publishSettings);
});

Task("Publish")
    .IsDependentOn("Pack")
    .Does(() => {
        var settings = new DotNetCoreNuGetPushSettings
        {
            Source = "https://api.nuget.org/v3/index.json",
            ApiKey = EnvironmentVariable("NUGET_API_KEY")
        };

        DotNetCoreNuGetPush($"{outputDir}/{nugetpackageId}.{version}.nupkg", settings);
});

private string GetBuildVersion(string productVersion)
{
    var now = DateTime.Now.ToString("yyyyMMddHHmmss");
    var shipit = Argument("shipit", "0") != "0";

    if(shipit)
    {
        return $"{productVersion}";
    }
    else
    {
        return $"{productVersion}-beta{now}";
    }
}

RunTarget(target);
