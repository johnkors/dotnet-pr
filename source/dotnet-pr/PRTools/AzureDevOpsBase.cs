namespace PR.PRTools;

public abstract class AzureDevOpsBase : IPRTool
{
    protected abstract string Host { get; }
    protected abstract string SshUrlFormat { get; }

    public bool IsMatch(string remoteUrl)
    {
        return remoteUrl.Contains(Host);
    }

    public string BuildUrl(GitContext gitContext)
    {
        string org;
        string project;
        string repo;

        if (gitContext.RemoteUrl.Contains(SshUrlFormat))
        {
            org = GetOrganization(gitContext.RemoteUrl);
            project = GetProject(gitContext.RemoteUrl);
            repo = GetRepo(gitContext.RemoteUrl);
        }
        else
        {
            org = GetOrganizationHttp(gitContext.RemoteUrl);
            project = GetProjectHttp(gitContext.RemoteUrl);
            repo = GetRepoHttp(gitContext.RemoteUrl);
        }

        return BuildUrl(org, project, repo, gitContext);
    }

    protected abstract string BuildUrl(string org, string project, string repo, GitContext gitContext);

    protected abstract string GetOrganizationHttp(string gitRemoteUrl);

    private string GetOrganization(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(':')[1];
        var org = gitUrl.Split('/')[1];
        return org;
    }

    private string GetProjectHttp(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(Host + "/")[1];
        var project = gitUrl.Split('/')[1];
        return project;
    }

    private string GetProject(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(':')[1];
        var project = gitUrl.Split('/')[2];
        return project;
    }

    private string GetRepoHttp(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(Host + "/")[1];
        var repo = gitUrl.Split('/')[3].Replace(".git", "");
        return repo;
    }

    private string GetRepo(string gitRemoteUrl)
    {
        var gitUrl = gitRemoteUrl.Split(':')[1];
        var repo = gitUrl.Split('/')[3].Replace(".git", "");
        return repo;
    }
}
