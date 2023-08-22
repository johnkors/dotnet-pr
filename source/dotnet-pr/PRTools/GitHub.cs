namespace PR.PRTools;

internal class GitHub : IPRTool
{
    /// <param name="remoteUrl">
    /// https://github.com/johnkors/dotnet-pr.git
    /// git@github.com:johnkors/dotnet-pr.git
    /// </param>
    public bool IsMatch(string remoteUrl)
    {
        return remoteUrl.Contains("github.com");
    }
        
    public string BuildUrl(GitContext gitContext)
    {
        if (gitContext.RemoteUrl.StartsWith("http"))
        {
            var accountAndRepo = gitContext.RemoteUrl.Split("github.com/")[1];
            var account = accountAndRepo.Split("/")[0];
            var repo = accountAndRepo.Split("/")[1].Replace(".git", "");
            return PrUrl(account, repo, gitContext.TargetBranch, gitContext.SourceBranch);
        }
        var accountAndRepoSsh = gitContext.RemoteUrl.Split("github.com:")[1];
        var accountSsh = accountAndRepoSsh.Split("/")[0];
        var repoSsh = accountAndRepoSsh.Split("/")[1].Replace(".git", "");
        return PrUrl(accountSsh, repoSsh, gitContext.TargetBranch, gitContext.SourceBranch);
    }
       
    private static string PrUrl(string account, string repo, string targetBranch, string sourceBranch)
    {
        return $"https://github.com/{account}/{repo}/compare/{targetBranch}...{sourceBranch}";
    }
}