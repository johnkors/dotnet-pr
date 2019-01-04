namespace pr
{
    public interface IVCSStrategy
    {
        string TransformToPRUrl(string gitRemoteUrl, string branch);
    }
}