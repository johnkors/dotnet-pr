using System;

namespace PR.PRTools
{
    internal class Gitlab : IPRTool
    {
        /// <param name="remoteUrl">
        /// https://gitlab.com/johnkors/dotnet-pr.git
        /// git@gitlab.com:johnkors/dotnet-pr.git
        /// </param>
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.Contains("@gitlab.com", StringComparison.InvariantCultureIgnoreCase);
        }

        public string BuildUrl(GitContext gitContext)
        {
            if (gitContext.RemoteUrl.StartsWith("http"))
            {
                var accountAndRepo = gitContext.RemoteUrl.Split("gitlab.com/")[1];
                var account = accountAndRepo.Split("/")[0];
                var repo = accountAndRepo.Split("/")[1].Replace(".git", "");
                return PrUrl(account, repo, gitContext.SourceBranch, gitContext.TargetBranch);
            }
            var accountAndRepoSsh = gitContext.RemoteUrl.Split("gitlab.com:")[1];
            var accountSsh = accountAndRepoSsh.Split("/")[0];
            var repoSsh = accountAndRepoSsh.Split("/")[1].Replace(".git", "");
            
            return PrUrl(accountSsh, repoSsh, gitContext.SourceBranch, gitContext.TargetBranch);
        }
       
        private static string PrUrl(string account, string repo, string sourceBranch, object targetBranch)
        {
            return $"https://gitlab.com/{account}/{repo}/merge_requests/new?merge_request%5Bsource_branch%5D={sourceBranch}&merge_request%5Btarget_branch%5D={targetBranch}";
        }
    }
}