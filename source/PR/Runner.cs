using System;

namespace PR
{
    internal class Runner
    {
        private readonly GitHelper _git;
        private readonly PRToolFactory _factory;
        private readonly Browser _browser;

        public Runner(GitHelper git, PRToolFactory factory, Browser browser)
        {
            _git = git;
            _factory = factory;
            _browser = browser;
        }
        public void Run()
        {
            var repo = _git.GetRepository();
            if (repo == null)
            {
                throw new ApplicationException("No Git repo found!");
            }

            var branch = repo.Head.FriendlyName;
            var remote = _git.ResolveRemote(repo, branch);

            var prTool = _factory.CreatePRTool(remote.Url);

            if (prTool == null)
            {
                throw new ApplicationException($"Unknown PR tool! Cannot not open PR UI for remote {remote.Name}. ");
            }

            var prUrl = prTool.CreatePRUrl(remote.Url, branch);
            _browser.Open(prUrl);
        }
    }
}