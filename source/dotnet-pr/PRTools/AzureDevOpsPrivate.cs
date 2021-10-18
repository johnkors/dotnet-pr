namespace PR.PRTools
{
    public class AzureDevOpsPrivate : AzureDevOpsBase
    {
        protected override string Host => ".visualstudio.com";

        protected override string SshUrlFormat => "@vs-ssh.visualstudio.com";

        protected override string BuildUrl(string org, string project, string repo, GitContext gitContext)
        {
            return $"https://{org}.visualstudio.com/{project}/_git/{repo}/pullrequestcreate?targetRef={gitContext.TargetBranch}&sourceRef={gitContext.SourceBranch}";
        }

        protected override string GetOrganizationHttp(string gitRemoteUrl)
        {
            const string scheme = "://";
            var schemeIdx = gitRemoteUrl.IndexOf(scheme);
            var gitRemoteUrlWithoutScheme = gitRemoteUrl.Substring(schemeIdx + scheme.Length);
            var orgIdx = gitRemoteUrlWithoutScheme.IndexOf('.');
            var org = gitRemoteUrlWithoutScheme.Substring(0, orgIdx);
            return org;
        }
    }
}