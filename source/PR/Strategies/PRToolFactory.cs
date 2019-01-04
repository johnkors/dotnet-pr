using System;
using System.Collections.Generic;
using System.Linq;

namespace PR
{
    internal class PRToolFactory
    {
        private IEnumerable<IVCSStrategy> _supportedStrategies;

        public PRToolFactory(IEnumerable<IVCSStrategy> strategies)
        {
            _supportedStrategies = strategies;
        }
        
        public IVCSStrategy CreatePRTool(string remoteUrl)
        {
            return _supportedStrategies.FirstOrDefault(s => s.IsMatch(remoteUrl));
        }
    }    
}