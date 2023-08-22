using System;

namespace PR.PRTools;

internal class BitBucketSelfHosted : IPRTool
{
    /// <param name="remoteUrl">
    /// http://bitbucket/scm/do/cmdline-utilities.git/
    /// ssh://git@bitbucket:7999/do/cmdline-utilities.git
    /// </param>
    public bool IsMatch(string remoteUrl)
    {
        var sshUrlMatch = remoteUrl.StartsWith("ssh://git@bitbucket", StringComparison.InvariantCultureIgnoreCase);
        var httpUrlMatch = remoteUrl.StartsWith("http://bitbucket/scm", StringComparison.InvariantCultureIgnoreCase);
        return sshUrlMatch || httpUrlMatch;
    }

    public string BuildUrl(GitContext gitContext)
    {
        var uri = new Uri(gitContext.RemoteUrl);
        if (uri.Scheme == "ssh")
        {
            return PrUrl(gitContext.SourceBranch, uri, ParseProjectFromSSHUri, ParseRepoFromSSHUri, gitContext.TargetBranch);
        }
        return PrUrl(gitContext.SourceBranch, uri, ParseProjectFromHttpUri, ParseRepoFromHttpUri, gitContext.TargetBranch);
    }

    private static string PrUrl(string branch, Uri uri, Func<Uri,string> ProjectFetcher, Func<Uri, string> RepoFetcher, string targetBranch)
    {
        return $"http://{uri.Host}/projects/{ProjectFetcher(uri)}/repos/{RepoFetcher(uri)}/compare/diff?sourceBranch={branch}&targetBranch={targetBranch}";
    }

    private static string ParseRepoFromSSHUri(Uri uri)
    {
        return uri.Segments[2].Replace(".git", "",StringComparison.InvariantCultureIgnoreCase).TrimStart('/').TrimEnd('/');
    }

    private static string ParseProjectFromSSHUri(Uri uri)
    {
        return uri.Segments[1].TrimStart('/').TrimEnd('/');
    }
        
    private static string ParseProjectFromHttpUri(Uri uri)
    {
        return uri.Segments[2].TrimStart('/').TrimEnd('/');
    }
        
    private static string ParseRepoFromHttpUri(Uri uri)
    {
        return uri.Segments[3].Replace(".git", "",StringComparison.InvariantCultureIgnoreCase).TrimStart('/').TrimEnd('/');
    }
}