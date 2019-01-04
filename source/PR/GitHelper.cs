using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace PR
{
    internal class GitHelper
    {
        private readonly string _initialDirectory;
        private readonly ILogger<GitHelper> _logger;
    
        public GitHelper(ILogger<GitHelper> logger)
        {
            _logger = logger;
            _initialDirectory = Directory.GetCurrentDirectory();

        }

        public Repository GetRepository()
        {
            return GetRepository(_initialDirectory);
        }

        private Repository GetRepository(string currentDirectory)
        {   
            _logger.LogDebug("Searching for git repo in " + currentDirectory);

            try
            {
                return new Repository(currentDirectory);
            }
            catch(RepositoryNotFoundException){
                

                try
                {
                    if (Directory.GetParent(currentDirectory) == null)
                    {
                        return null;
                    }

                    if(!Directory.GetParent(currentDirectory).Exists)
                    {
                        return null;
                    }
                }
                catch(Exception)
                {
                    return null;
                }

                var parentDir = Directory.GetParent(currentDirectory).FullName;
                return GetRepository(parentDir);
            }
        }
        
        public Remote ResolveRemote(Repository repo, string localBranch)
        {
            var remoteName = "origin";
            if (repo.Head.TrackedBranch != null)
            {
                remoteName = repo.Head.TrackedBranch.RemoteName;
                _logger.LogDebug("Found tracking branch. Using connected remote.");
            }
            else
            {
                _logger.LogDebug("No tracking branch found. Searching remotes.");

                foreach (var repoBranch in repo.Branches.Where(r => r.IsRemote))
                {
                    var shortName = repoBranch.FriendlyName.Replace($"{repoBranch.RemoteName}/", "");
                    if (shortName == localBranch)
                    {
                        _logger.LogDebug($"Found branch at remote {repoBranch.RemoteName} : {repoBranch.FriendlyName}.");
                        remoteName = repoBranch.RemoteName;
                    }
                }
            }

            var remote = repo.Network.Remotes.First(r => r.Name == remoteName);
            _logger.LogDebug($"Using {remoteName}");
            return remote;
        }
    }
}