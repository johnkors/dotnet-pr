namespace PR.PRTools
{
    internal class GitHub : IPRTool
    {
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.StartsWith("git@github.com");
        }

        public string CreatePRUrl(PRInfo prInfo)
        {
            return $"https://github.com/{GetOrganization(prInfo.RemoteUrl)}/{GetRepo(prInfo.RemoteUrl)}/compare/master...{prInfo.BranchName}";
        }

        private string GetRepo(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[1].Replace(".git", "");
            return repo;
        }

        private string GetOrganization(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[0];
            return repo;
        }
    }
}