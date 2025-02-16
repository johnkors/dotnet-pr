using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace PR;

internal class GitRepositoryLocator(ILogger<GitRepositoryLocator> logger)
{
    private readonly string _initialDirectory = Directory.GetCurrentDirectory();

    public Repository LocateRepository()
    {
        var repository = LocateRepository(_initialDirectory);
        if (repository == null)
        {
            throw new ApplicationException("No Git repo found!");
        }

        return repository;
    }

    private Repository? LocateRepository(string currentDirectory)
    {
        logger.LogDebug("Searching for git repo in " + currentDirectory);

        try
        {
            return new Repository(currentDirectory);
        }
        catch (RepositoryNotFoundException)
        {
            var currentDir = Directory.GetParent(currentDirectory);
            try
            {
                if (currentDir == null)
                {
                    return null;
                }

                if (!currentDir.Exists)
                {
                    return null;
                }
            }
            catch (Exception)
            {
                return null;
            }

            var parentDir = currentDir.FullName;
            return LocateRepository(parentDir);
        }
    }
}
