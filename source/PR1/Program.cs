using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace PR
{
    class Program
    {
        static void Main()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            Console.WriteLine($"pwd: {currentDirectory}");
            var repo = GitHelper.GetRepository(currentDirectory);
            if (repo == null)
            {
                Console.WriteLine("No Git repo found!");
                Environment.Exit(-1);
            }

            var branch = repo.Head.FriendlyName;
            
            var remote = GitHelper.ResolveRemote(repo, branch);


            var strategy = StrategyHelper.GetVCSStrategy(remote.Url);

            if (strategy != null)
            {
                var prUrl = strategy.TransformToPRUrl(remote.Url, branch);
                Browser.Open(prUrl);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Unknown VCS code review tool for remote {remote.Name}");
                Environment.Exit(-1);
            }
        }
    }
}
