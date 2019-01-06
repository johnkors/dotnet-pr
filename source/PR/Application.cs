using System;
using System.Collections.Generic;
using System.Linq;
using PR.PRTools;

namespace PR
{
    internal class Application
    {
        private readonly GitRepositoryLocator _repositoryLocator;
        private readonly GitRemoteGuesser _remoteGuesser;
        private readonly IEnumerable<IPRTool> _tools;
        private readonly Browser _browser;

        public Application(GitRepositoryLocator repositoryLocator, GitRemoteGuesser remoteGuesser, IEnumerable<IPRTool> tools, Browser browser)
        {
            _repositoryLocator = repositoryLocator;
            _remoteGuesser = remoteGuesser;
            _tools = tools;
            _browser = browser;
        }
        
        public void OpenToolInBrowser()
        {
            var repo = _repositoryLocator.LocateRepository();
            var remote = _remoteGuesser.GetGitRemote(repo);
            var prTool = _tools.FirstOrDefault(s => s.IsMatch(remote.Url));

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
                TargetBranch = "master"
            });
            
            _browser.Open(prUrl);
        }
    }
}