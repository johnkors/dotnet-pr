namespace PR;

public class GitContext
{
    public required string RemoteUrl { get; init; }
    public required string SourceBranch { get; init; }
    public required string TargetBranch { get; init; }
}
