using System.Collections.Generic;
using System.Linq;

namespace PR.PRTools
{
    internal class PRToolFactory
    {
        private IEnumerable<IPRTool> _supportedStrategies;

        public PRToolFactory(IEnumerable<IPRTool> strategies)
        {
            _supportedStrategies = strategies;
        }
        
        public IPRTool CreatePRTool(string remoteUrl)
        {
            return _supportedStrategies.FirstOrDefault(s => s.IsMatch(remoteUrl));
        }
    }    
}