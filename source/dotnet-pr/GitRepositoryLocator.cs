using System;
using System.IO;
using LibGit2Sharp;
using Microsoft.Extensions.Logging;

namespace PR
{
    internal class GitRepositoryLocator
    {
        private readonly string _initialDirectory;
        private readonly ILogger<GitRepositoryLocator> _logger;
    
        public GitRepositoryLocator(ILogger<GitRepositoryLocator> logger)
        {
            _logger = logger;
            _initialDirectory = Directory.GetCurrentDirectory();
        }

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
            _logger.LogDebug("Searching for git repo in " + currentDirectory);

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
}