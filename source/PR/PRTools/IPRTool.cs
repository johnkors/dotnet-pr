namespace PR.PRTools
{
    internal interface IPRTool
    {
        bool IsMatch(string remoteUrl);
        string CreatePRUrl(GitContext prinfo);
    }
}