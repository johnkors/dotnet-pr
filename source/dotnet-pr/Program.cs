using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PR;
using PR.PRTools;

using var serviceProvider = Bootstrap(args);
var runner = serviceProvider.GetService<Application>();

try
{
    runner.OpenToolInBrowser();
}
catch (ApplicationException ae)
{
    var logger = serviceProvider.GetService<ILogger<Program>>();
    logger.LogError($"¯\\_(ツ)_/¯ \n{ae.Message}");
}
catch (Exception e)
{
    var logger = serviceProvider.GetService<ILogger<Program>>();
    logger.LogError($"¯\\_(ツ)_/¯ \n{e.Message}");
}

ServiceProvider Bootstrap(string[] args)
{
    var enableDebug = args.Contains("--debug");
    var targetBranch = string.Empty;
    if (args.Length > 0)
    {
        var restOfArgs = args.ToList();
        restOfArgs.Remove("--debug");
        targetBranch = restOfArgs.Any() ? restOfArgs[0] : "master";
    }
    else
    {
        targetBranch = "master";
    }
    var debugOptions = new AppOptions
    {
        EnableDebug = enableDebug,
        TargetBranch = targetBranch
    };

    var services = new ServiceCollection()
        .AddLogging(c =>
        {
            c.AddConsole().AddDebug();

            if (debugOptions.EnableDebug)
            {
                c.SetMinimumLevel(LogLevel.Trace);
            }
            else
            {
                c.SetMinimumLevel(LogLevel.Warning);
            }

        })
        .AddSingleton(debugOptions)
        .AddSingleton<GitRepositoryLocator>()
        .AddSingleton<GitRemoteGuesser>()
        .AddSingleton<IPRTool, BitBucketSelfHosted>()
        .AddSingleton<IPRTool, GitHub>()
        .AddSingleton<IPRTool, BitBucketOrg>()
        .AddSingleton<IPRTool, Gitlab>()
        .AddSingleton<IPRTool, AzureDevOps>()
        .AddSingleton<IPRTool, AzureDevOpsPrivate>()
        .AddSingleton<Browser>()
        .AddSingleton<Application>();

    return services.BuildServiceProvider();
}
