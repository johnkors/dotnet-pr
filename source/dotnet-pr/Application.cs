using Microsoft.Extensions.Logging;
using PR.PRTools;

namespace PR;

internal class Application(GitRepositoryLocator repositoryLocator,
    GitRemoteGuesser remoteGuesser,
    IEnumerable<IPRTool> tools,
    AppOptions options,
    Browser browser, ILoggerFactory logFactory)
{
    public void OpenToolInBrowser()
    {
        var repo = repositoryLocator.LocateRepository();
        var remote = remoteGuesser.GetGitRemote(repo);
        var prTool = tools.FirstOrDefault(s =>
        {
            var logger = logFactory.CreateLogger(s.GetType());
            var isMatch = s.IsMatch(remote.Url);
            logger.LogDebug(isMatch ? $"Match" : "No match");
            return isMatch;
        });

        if (prTool == null)
        {
            var supportedList = tools
                .OrderBy(t => t.GetType().Name)
                .Select(c => $"\n* {c.GetType().Name.ToString()}")
                .Aggregate((x,y) => x + y);

            throw new ApplicationException($"Unknown PR tool. Could not open PR for the `{remote.Name}` remote. \nSupported tools : {supportedList}");
        }

        var prUrl = prTool.BuildUrl(new GitContext
        {
            RemoteUrl = remote.Url,
            SourceBranch = repo.Head.FriendlyName,
            TargetBranch = options.TargetBranch
        });

        browser.Open(prUrl);
    }
}
