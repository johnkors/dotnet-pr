using System.IO;
using LibGit2Sharp;

namespace pr
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
                var r =  new Repository(currentDirectory);
                return r;
            }
            catch(RepositoryNotFoundException){
                var parentDirectory = Directory.GetParent(currentDirectory);
                return GetRepository(parentDirectory.FullName);
            }
        }
    }
}