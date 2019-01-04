using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;

namespace PR
{
    internal static class GitHelper
    {
        public static Repository GetRepository(string currentDirectory)
        {
            if (!Directory.GetParent(currentDirectory).Exists)
            {
                return null;
            }
            
            try{
                return new Repository(currentDirectory);
            }
            catch(RepositoryNotFoundException){
                return GetRepository(Directory.GetParent(currentDirectory).FullName);
            }
        }
        
        public static Remote ResolveRemote(Repository repo, string branch)
        {
            string remoteName = "origin";
            if (repo.Head.TrackedBranch != null)
            {
                remoteName = repo.Head.TrackedBranch.RemoteName;
                Console.WriteLine("Found tracking branch. Using connected remote.");
            }
            else
            {
                Console.WriteLine("No tracking branch. Searching for pushed branches at remotes.");

                foreach (var repoBranch in repo.Branches.Where(r => r.IsRemote))
                {
                    if (repoBranch.FriendlyName.Replace($"{repoBranch.RemoteName}/", "") == branch)
                    {
                        Console.WriteLine($"Found branch at remote : {repoBranch.FriendlyName}.");
                        remoteName = repoBranch.RemoteName;
                    }
                }
            }

            return repo.Network.Remotes.First(r => r.Name == remoteName);
        }
    }
}