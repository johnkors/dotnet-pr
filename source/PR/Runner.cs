using System;
using PR.PRTools;

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
            _git.LocateRepository();

            var prInfo = _git.GuessRemoteForPR();

            var prTool = _factory.CreatePRTool(prInfo.RemoteUrl);

            if (prTool == null)
            {
                throw new ApplicationException($"Unknown PR tool! Cannot not open PR UI for remote {prInfo.RemoteName}. ");
            }

            var prUrl = prTool.CreatePRUrl(prInfo);
            _browser.Open(prUrl);
        }
    }
}