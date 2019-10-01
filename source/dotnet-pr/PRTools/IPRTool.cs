namespace PR.PRTools
{
    public interface IPRTool
    {
        bool IsMatch(string remoteUrl);
        string BuildUrl(GitContext gitContext);
    }
}