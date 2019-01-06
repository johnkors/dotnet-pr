using System.Linq;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace PR
{
    internal class GitRemoteGuesser
    {
        private readonly ILogger<GitRemoteGuesser> _logger;

        public GitRemoteGuesser(ILogger<GitRemoteGuesser> logger)
        {
            _logger = logger;
        }
        
        public Remote GetGitRemote(Repository repository)
        {
            var localBranch = repository.Head.FriendlyName;

            var remoteName = "origin"; //default
            if (repository.Head.TrackedBranch != null)
            {
                remoteName = repository.Head.TrackedBranch.RemoteName;
                _logger.LogDebug($"Found tracking branch. Using connected remote {remoteName}");
            }
            else
            {
                _logger.LogDebug("No tracking branch found. Searching remotes.");

                foreach (var repoBranch in repository.Branches.Where(r => r.IsRemote))
                {
                    var shortName = repoBranch.FriendlyName.Replace($"{repoBranch.RemoteName}/", "");
                    if (shortName == localBranch)
                    {
                        _logger.LogDebug($"Found branch at remote {repoBranch.RemoteName} : {repoBranch.FriendlyName}.");
                        remoteName = repoBranch.RemoteName;
                    }
                }
            }

            var remote = repository.Network.Remotes.First(r => r.Name == remoteName);
            _logger.LogDebug($"Using the {remoteName} remote");

            return remote;
        }
    }
}