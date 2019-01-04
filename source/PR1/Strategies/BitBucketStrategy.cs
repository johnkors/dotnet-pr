using System;

namespace PR
{
    internal class BitBucketStrategy : IVCSStrategy
    {
        public bool IsMatch(string remoteUrl)
        {
            Uri theUri;
            var couldCreate = Uri.TryCreate(remoteUrl, UriKind.Absolute, out theUri);
            if (couldCreate)
            {
                var sshUrlMatch = remoteUrl.StartsWith("ssh://git@bitbucket", StringComparison.InvariantCultureIgnoreCase);
                var httpUrlMatch = remoteUrl.StartsWith("http://bitbucket/scm", StringComparison.InvariantCultureIgnoreCase);
                return sshUrlMatch || httpUrlMatch;
            }
                
            return false;
        }

        public string TransformToPRUrl(string gitRemoteUrl, string currentBranch)
        {
            var uri = new Uri(gitRemoteUrl);
            if (uri.Scheme == "ssh")
            {
                return PrUrl(currentBranch, uri, ParseProjectFromSSHUri, ParseRepoFromSSHUri);
            }
            return PrUrl(currentBranch, uri, ParseProjectFromHttpUri, ParseRepoFromHttpUri);
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