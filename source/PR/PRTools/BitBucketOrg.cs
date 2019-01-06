using System;

namespace PR.PRTools
{
    internal class BitBucketOrg : IPRTool
    {
        /// <param name="remoteUrl">
        /// https://johnkors@bitbucket.org/johnkors/dotnet-pr.git
        /// git@bitbucket.org:johnkors/dotnet-pr.git
        /// </param>
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("@bitbucket.org", StringComparison.InvariantCultureIgnoreCase);
        }

        public string BuildUrl(GitContext gitContext)
        {
            if (gitContext.RemoteUrl.StartsWith("http"))
            {
                var accountAndRepo = gitContext.RemoteUrl.Split("bitbucket.org/")[1];
                var account = accountAndRepo.Split("/")[0];
                var repo = accountAndRepo.Split("/")[1].Replace(".git", "");
                return PrUrl(account, repo, gitContext.SourceBranch);
            }
            var accountAndRepoSsh = gitContext.RemoteUrl.Split("bitbucket.org:")[1];
            var accountSsh = accountAndRepoSsh.Split("/")[0];
            var repoSsh = accountAndRepoSsh.Split("/")[1].Replace(".git", "");
            return PrUrl(accountSsh, repoSsh, gitContext.SourceBranch);
        }
       
        private static string PrUrl(string account, string repo, string branch)
        {
            return $"https://bitbucket.org/{account}/{repo}/pull-requests/new?source={branch}&t=1";
        }
    }
}