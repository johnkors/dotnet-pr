using System;

namespace PR.PRTools
{
    internal class GitLab : IPRTool
    {
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("@gitlab.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public string CreatePRUrl(PRInfo PRinfo)
        {
            if (PRinfo.RemoteUrl.StartsWith("http"))
            {
                //https://gitlab.com/johnkors/dotnet-pr.git
                var accountAndRepo = PRinfo.RemoteUrl.Split("gitlab.com/")[1];
                var account = accountAndRepo.Split("/")[0];
                var repo = accountAndRepo.Split("/")[1].Replace(".git", "");
                return PrUrl(account, repo, PRinfo.BranchName);
            }
            // git@gitlab.com:johnkors/dotnet-pr.git
            var accountAndRepoSsh = PRinfo.RemoteUrl.Split("gitlab.com:")[1];
            var accountSsh = accountAndRepoSsh.Split("/")[0];
            var repoSsh = accountAndRepoSsh.Split("/")[1].Replace(".git", "");
            return PrUrl(accountSsh, repoSsh, PRinfo.BranchName);
        }
       
        private static string PrUrl(string account, string repo, string branch)
        {
            return $"https://gitlab.com/{account}/{repo}/merge_requests/new?merge_request%5Bsource_branch%5D={branch}";
        }
    }
}