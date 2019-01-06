namespace PR
{
    internal class GitContext
    {
        public string RemoteUrl { get; set; }
        public string SourceBranch { get; set; }
        public string TargetBranch { get; set; } = "master";
    }
}