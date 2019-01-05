using System;

namespace PR.PRTools
{
    internal class BitBucketOrg : IPRTool
    {
        public bool IsMatch(string remoteUrl)
        {
            return remoteUrl.StartsWith("git@bitbucket.org", StringComparison.InvariantCultureIgnoreCase);
        }

        public string CreatePRUrl(PRInfo PRinfo)
        {
            var uri = new Uri(PRinfo.RemoteUrl);
            if (uri.Scheme == "ssh")
            {
                return PrUrl(PRinfo.BranchName, uri, ParseProjectFromSSHUri, ParseRepoFromSSHUri);
            }
            return PrUrl(PRinfo.BranchName, uri, ParseProjectFromHttpUri, ParseRepoFromHttpUri);
        }

        private static string PrUrl(string branch, Uri uri, Func<Uri,string> ProjectFetcher, Func<Uri, string> RepoFetcher)
        {
            return $"http://{uri.Host}/projects/{ProjectFetcher(uri)}/repos/{RepoFetcher(uri)}/compare/commits?sourceBranch={branch}";
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
}