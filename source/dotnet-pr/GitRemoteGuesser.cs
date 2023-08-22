using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace PR;

internal class GitRemoteGuesser(ILogger<GitRemoteGuesser> logger)
{
    public Remote GetGitRemote(Repository repository)
    {
        var localBranch = repository.Head.FriendlyName;

        var remoteName = "origin"; //default
        if (repository.Head.TrackedBranch != null)
        {
            remoteName = repository.Head.TrackedBranch.RemoteName;
            logger.LogDebug($"Found tracking branch. Using connected remote {remoteName}");
        }
        else
        {
            logger.LogDebug("No tracking branch found. Searching remotes.");

            foreach (var repoBranch in repository.Branches.Where(r => r.IsRemote))
            {
                var shortName = repoBranch.FriendlyName.Replace($"{repoBranch.RemoteName}/", "");
                if (shortName == localBranch)
                {
                    logger.LogDebug($"Found branch at remote {repoBranch.RemoteName} : {repoBranch.FriendlyName}.");
                    remoteName = repoBranch.RemoteName;
                }
            }
        }

        var remote = repository.Network.Remotes.First(r => r.Name == remoteName);
        logger.LogDebug($"Using the {remoteName} remote");

        return remote;
    }
}
