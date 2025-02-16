namespace PR.PRTools;

public class AzureDevOps : AzureDevOpsBase
{
    protected override string Host => "dev.azure.com";

    protected override string SshUrlFormat => "git@ssh";

    protected override string BuildUrl(string org, string project, string repo, GitContext gitContext)
    {
        return
            $"https://dev.azure.com/{org}/{project}/_git/{repo}/pullrequestcreate?targetRef={gitContext.TargetBranch}&sourceRef={gitContext.SourceBranch}";
    }

    protected override string GetOrganizationHttp(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(Host + "/")[1];
        var repo = gitUrl.Split('/')[0];
        return repo;
    }
}
