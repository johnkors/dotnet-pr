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

        public Repository Repository { get; set; }

        public void LocateRepository()
        {
            Repository = LocateRepository(_initialDirectory);
            if (Repository == null)
            {
                throw new ApplicationException("No Git repo found!");
            }
        }

        public PRInfo GuessRemoteForPR()
        {
            var localBranch = Repository.Head.FriendlyName;

            var remoteName = "origin"; //default
            if (Repository.Head.TrackedBranch != null)
            {
                remoteName = Repository.Head.TrackedBranch.RemoteName;
                _logger.LogDebug($"Found tracking branch. Using connected remote {remoteName}");
            }
            else
            {
                _logger.LogDebug("No tracking branch found. Searching remotes.");

                foreach (var repoBranch in Repository.Branches.Where(r => r.IsRemote))
                {
                    var shortName = repoBranch.FriendlyName.Replace($"{repoBranch.RemoteName}/", "");
                    if (shortName == localBranch)
                    {
                        _logger.LogDebug($"Found branch at remote {repoBranch.RemoteName} : {repoBranch.FriendlyName}.");
                        remoteName = repoBranch.RemoteName;
                    }
                }
            }

            var remote = Repository.Network.Remotes.First(r => r.Name == remoteName);
            _logger.LogDebug($"Using the {remoteName} remote");
            
            return new PRInfo
            {
                RemoteName = remote.Name,
                RemoteUrl = remote.Url,
                BranchName = localBranch
            };
        }

        private Repository LocateRepository(string currentDirectory)
        {   
            _logger.LogDebug("Searching for git repo in " + currentDirectory);

            try
            {
                return new Repository(currentDirectory);
            }
            catch(RepositoryNotFoundException)
            {
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
                return LocateRepository(parentDir);
            }
        }
    }
}