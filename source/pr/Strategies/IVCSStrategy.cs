namespace PR
{
    public interface IVCSStrategy
    {
        string TransformToPRUrl(string gitRemoteUrl, string currentBranch);
        bool IsMatch(string remoteUrl);
    }
}