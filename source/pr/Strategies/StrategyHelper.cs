using System;
using System.Collections.Generic;
using System.Linq;

namespace pr
{
    internal class StrategyHelper
    {

        private static IEnumerable<IVCSStrategy> SupportedStrategies = new List<IVCSStrategy>
        {
            new BitBucketStrategy()
        };
        
        public static IVCSStrategy GetVCSStrategy(string remoteUrl)
        {
            var host = new Uri(remoteUrl).Host;

            if (host.Equals("bitbucket", StringComparison.InvariantCultureIgnoreCase))
                return SupportedStrategies.First(s => s is BitBucketStrategy);

            return null;
        }
    }    
}