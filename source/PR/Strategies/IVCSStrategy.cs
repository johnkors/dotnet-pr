namespace PR
{
    public interface IVCSStrategy
    {
        string CreatePRUrl(string gitRemoteUrl, string currentBranch);
        bool IsMatch(string remoteUrl);
    }
}