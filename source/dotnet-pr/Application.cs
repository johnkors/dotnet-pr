using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using PR.PRTools;

namespace PR;

internal class Application
{
    private readonly GitRepositoryLocator _repositoryLocator;
    private readonly GitRemoteGuesser _remoteGuesser;
    private readonly IEnumerable<IPRTool> _tools;
    private readonly AppOptions _options;
    private readonly Browser _browser;
    private readonly ILoggerFactory _logFactory;

    public Application(GitRepositoryLocator repositoryLocator,
        GitRemoteGuesser remoteGuesser,
        IEnumerable<IPRTool> tools,
        AppOptions options,
        Browser browser, ILoggerFactory logFactory)
    {
        _repositoryLocator = repositoryLocator;
        _remoteGuesser = remoteGuesser;
        _tools = tools;
        _options = options;
        _browser = browser;
        _logFactory = logFactory;
    }

    public void OpenToolInBrowser()
    {
        var repo = _repositoryLocator.LocateRepository();
        var remote = _remoteGuesser.GetGitRemote(repo);
        var prTool = _tools.FirstOrDefault(s =>
        {
            var logger = _logFactory.CreateLogger(s.GetType());
            var isMatch = s.IsMatch(remote.Url);
            logger.LogDebug(isMatch ? $"Match" : "No match");
            return isMatch;
        });

        if (prTool == null)
        {
            var supportedList = _tools
                .OrderBy(t => t.GetType().Name)
                .Select(c => $"\n* {c.GetType().Name.ToString()}")
                .Aggregate((x,y) => x + y);

            throw new ApplicationException($"Unknown PR tool. Could not open PR for the `{remote.Name}` remote. \nSupported tools : {supportedList}");
        }

        var prUrl = prTool.BuildUrl(new GitContext
        {
            RemoteUrl = remote.Url,
            SourceBranch = repo.Head.FriendlyName,
            TargetBranch = _options.TargetBranch
        });

        _browser.Open(prUrl);
    }
}
