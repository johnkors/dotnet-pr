using System.IO;
using LibGit2Sharp;

namespace PR
{
    internal static class GitHelper
    {
        public static Repository GetRepository(string currentDirectory)
        {
            if (!Directory.GetParent(currentDirectory).Exists)
            {
                return null;
            }
            
            try{
                return new Repository(currentDirectory);
            }
            catch(RepositoryNotFoundException){
                return GetRepository(Directory.GetParent(currentDirectory).FullName);
            }
        }
    }
}