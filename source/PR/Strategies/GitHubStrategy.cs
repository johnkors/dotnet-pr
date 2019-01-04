using System;

namespace PR
{
    public class GitHubStrategy : IVCSStrategy
    {
        /// <summary>
        /// git@github.com:johnkors/cmdline-utilities.git
        /// </summary>
        public string TransformToPRUrl(string gitRemoteUrl, string currentBranch)
        {
            return $"https://github.com/{GetOrganization(gitRemoteUrl)}/{GetRepo(gitRemoteUrl)}/compare/master...{currentBranch}";
        }

        private string GetRepo(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[1].Replace(".git", "");
            return repo;
        }
    
        private string GetOrganization(string gitRemoteUrl)
        {
            var gitUrl = gitRemoteUrl.Split(':')[1];
            var repo = gitUrl.Split('/')[0];
            return repo;
        }

        public bool IsMatch(string remoteUrl)
        {
            Console.WriteLine(remoteUrl);
            return remoteUrl.StartsWith("git@github.com");
        }
    }
}