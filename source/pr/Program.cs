using System;
using System.IO;
using System.Linq;

namespace pr
{
    class Program
    {
        static void Main()
        {
            var repo = GitHelper.GetRepository(Directory.GetCurrentDirectory());
            if (repo == null)
            {
                Console.WriteLine("No Git repo found!");
                Environment.Exit(-1);
                return;
            }

            var branch = repo.Head.FriendlyName;
            var remoteTrackedBranch = repo.Head.TrackedBranch.RemoteName;
            var remote = repo.Network.Remotes.First(r => r.Name.Equals(remoteTrackedBranch));

            var strategy = StrategyHelper.GetVCSStrategy(remote.Url);

            if (strategy != null)
            {
                var prUrl = strategy.TransformToPRUrl(remote.Url, branch);
                Browser.Open(prUrl);
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine($"Unknown VCS code review tool for remote {remoteTrackedBranch}");
                Environment.Exit(-1);
            }
            
        }
    }
}
