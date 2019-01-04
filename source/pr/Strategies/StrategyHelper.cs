using System;
using System.Collections.Generic;
using System.Linq;

namespace PR
{
    internal class StrategyHelper
    {

        private static IEnumerable<IVCSStrategy> SupportedStrategies = new List<IVCSStrategy>
        {
            new BitBucketStrategy()
        };
        
        public static IVCSStrategy GetVCSStrategy(string remoteUrl)
        {
            return SupportedStrategies.FirstOrDefault(s => s.IsMatch(remoteUrl));
        }
    }    
}