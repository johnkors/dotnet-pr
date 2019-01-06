namespace PR.PRTools
{
    internal class AzureDevOps : IPRTool
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="remoteUrl">
        /// https://RetireNET@dev.azure.com/RetireNET/dotnet-retire/_git/testrepo
        /// git@ssh.dev.azure.com:v3/RetireNET/dotnet-retire/testrepo
        /// </param>
        /// <returns></returns>
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("dev.azure.com");
        }

        public string CreatePRUrl(PRInfo prInfo)
        {
            if (prInfo.RemoteUrl.StartsWith("git@ssh"))
            {
                return $"https://dev.azure.com/{GetOrganization(prInfo.RemoteUrl)}/_git/{GetRepo(prInfo.RemoteUrl)}/pullrequestcreate?targetRef=master&sourceRef={prInfo.BranchName}";
            }
            return $"https://dev.azure.com/{GetOrganizationHttp(prInfo.RemoteUrl)}/_git/{GetRepoHttp(prInfo.RemoteUrl)}/pullrequestcreate?targetRef=master&sourceRef={prInfo.BranchName}";
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