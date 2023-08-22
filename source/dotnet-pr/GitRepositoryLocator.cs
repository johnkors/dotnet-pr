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

    private Repository LocateRepository(string currentDirectory)
    {
        logger.LogDebug("Searching for git repo in " + currentDirectory);

        try
        {
            return new Repository(currentDirectory);
        }
        catch(RepositoryNotFoundException)
        {
            try
            {
                if (Directory.GetParent(currentDirectory) == null)
                {
                    return null;
                }

                if(!Directory.GetParent(currentDirectory).Exists)
                {
                    return null;
                }
            }
            catch(Exception)
            {
                return null;
            }

            var parentDir = Directory.GetParent(currentDirectory).FullName;
            return LocateRepository(parentDir);
        }
    }
}
