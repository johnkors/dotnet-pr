using System;

namespace PR.PRTools
{
    internal class BitBucketOrg : IPRTool
    {
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("@bitbucket.org", StringComparison.InvariantCultureIgnoreCase);
        }

        public string CreatePRUrl(PRInfo PRinfo)
        {
            if (PRinfo.RemoteUrl.StartsWith("http"))
            {
                //https://johnkors@bitbucket.org/johnkors/dotnet-pr.git
                var accountAndRepo = PRinfo.RemoteUrl.Split("bitbucket.org/")[1];
                var account = accountAndRepo.Split("/")[0];
                var repo = accountAndRepo.Split("/")[1].Replace(".git", "");
                return PrUrl(account, repo, PRinfo.BranchName);
            }
            // git@bitbucket.org:johnkors/dotnet-pr.git
            var accountAndRepoSsh = PRinfo.RemoteUrl.Split("bitbucket.org:")[1];
            var accountSsh = accountAndRepoSsh.Split("/")[0];
            var repoSsh = accountAndRepoSsh.Split("/")[1].Replace(".git", "");
            return PrUrl(accountSsh, repoSsh, PRinfo.BranchName);
        }
       
        private static string PrUrl(string account, string repo, string branch)
        {
            return $"https://bitbucket.org/{account}/{repo}/pull-requests/new?source={branch}&t=1";
        }
    }
}