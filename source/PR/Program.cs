using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PR.PRTools;

namespace PR
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var serviceProvider = Bootstrap(args))
            {
                var runner = serviceProvider.GetService<Runner>();
                try
                {
                    runner.Run();
                }
                catch (ApplicationException ae)
                {
                    var logger = serviceProvider.GetService<ILogger<Program>>();
                    logger.LogError(ae.Message);
                }
            }
        }

        private static ServiceProvider Bootstrap(string[] args)
        {
            var enableDebug = args.Contains("--debug");
            var debugOptions = new DebugOptions { EnableDebug = enableDebug };

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
                .AddSingleton<GitHelper>()
                .AddSingleton<IPRTool,BitBucketSelfHosted>()
                .AddSingleton<IPRTool,GitHub>()
                .AddSingleton<IPRTool,BitBucketOrg>()
                .AddSingleton<PRToolFactory>()
                .AddSingleton<Browser>()
                .AddSingleton<Runner>();

            return services.BuildServiceProvider();
        }
    }
}
