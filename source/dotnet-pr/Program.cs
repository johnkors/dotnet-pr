﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PR;
using PR.PRTools;

using var serviceProvider = Bootstrap(args);
var runner = serviceProvider.GetRequiredService<Application>();

try
{
    runner.OpenToolInBrowser();
}
catch (ApplicationException ae)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError($"¯\\_(ツ)_/¯ \n{ae.Message}");
}
catch (Exception e)
{
    var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
    logger.LogError($"¯\\_(ツ)_/¯ \n{e.Message}");
}

return;

ServiceProvider Bootstrap(string[] args)
{
    var enableDebug = args.Contains("--debug");
    string targetBranch;
    if (args.Length > 0)
    {
        var restOfArgs = args.ToList();
        restOfArgs.Remove("--debug");
        targetBranch = restOfArgs.Any() ? restOfArgs[0] : "main";
    }
    else
    {
        targetBranch = "main";
    }

    var debugOptions = new AppOptions { EnableDebug = enableDebug, TargetBranch = targetBranch };

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
