namespace PR.PRTools
{
    internal class AzureDevOps : IPRTool
    {
        /// <param name="remoteUrl">
        /// https://RetireNET@dev.azure.com/RetireNET/dotnet-retire/_git/testrepo
        /// git@ssh.dev.azure.com:v3/RetireNET/dotnet-retire/testrepo
        /// </param>
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("dev.azure.com");
        }

        public string CreatePRUrl(GitContext gitContext)
        {
            if (gitContext.RemoteUrl.StartsWith("git@ssh"))
            {
                return $"https://dev.azure.com/{GetOrganization(gitContext.RemoteUrl)}/_git/{GetRepo(gitContext.RemoteUrl)}/pullrequestcreate?targetRef=master&sourceRef={gitContext.BranchName}";
            }
            return $"https://dev.azure.com/{GetOrganizationHttp(gitContext.RemoteUrl)}/_git/{GetRepoHttp(gitContext.RemoteUrl)}/pullrequestcreate?targetRef=master&sourceRef={gitContext.BranchName}";
        }
        
        private string GetOrganizationHttp(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split("dev.azure.com/")[1];
            var repo = gitUrl.Split('/')[0];
            return repo;
        }
        
        private string GetRepoHttp(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split("dev.azure.com/")[1];
            var repo = gitUrl.Split('/')[3].Replace(".git", "");
            return repo;
        }

        private string GetOrganization(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[1];
            return repo;
        }

        private string GetRepo(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[3].Replace(".git", "");
            return repo;
        }
    }
}