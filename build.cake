var target = Argument("target", "Pack");
var project = "dotnet-pr";
var version = GetBuildVersion("1.8.1");
var outputDir = $"./builds/{project}";

Task("Test")
    .Does(() => {
        var settings = new DotNetCoreTestSettings
        {
            Configuration = "Release",
                ArgumentCustomization = args=>args.Append($"--logger console;verbosity=detailed")
        };            
        DotNetCoreTest("./source/dotnet-pr.tests/dotnet-pr.tests.csproj", settings); 
});

Task("Pack")
    .IsDependentOn("Test")
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

        DotNetCoreNuGetPush($"{outputDir}/{project}.{version}.nupkg", settings);
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
