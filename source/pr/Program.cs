using System;
using System.IO;
using System.Linq;

namespace pr
{
    class Program
    {
        static void Main(string[] args)
        {
            var repo = GitHelper.GetRepository(Directory.GetCurrentDirectory());
            if (repo != null)
            {
                var branch = repo.Head.FriendlyName;
                
                foreach (var remote in repo.Network.Remotes)
                {
                    if (BitBucketHelper.IsBitBucketUrl(remote.Url))
                    {
                        var prUrl = BitBucketHelper.TransformToPRUrl(remote.Url, branch);
                        Browser.Open(prUrl);
                        Environment.Exit(0);
                    }
                }
            }
            else
            {
                Console.Warning("No Git repo found!");
                Environment.Exit(-1);
            }
        }
    }
}
