using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;

namespace PR
{
    internal class Browser
    {
        private readonly bool _debug;
        private readonly ILogger<Browser> _logger;

        public Browser(ILogger<Browser> logger, DebugOptions debugOptions)
        {
            _logger = logger;
            _debug = debugOptions.EnableDebug;
        }

        public void Open(string url)
        {
            if (_debug)
            {
                _logger.LogDebug(url);
            }
            else
            {
                #if RELEASE
                OpenBrowserAt(url);
                #else
                _logger.LogDebug(url);
                #endif
            }
        }

        private static void OpenBrowserAt(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") {CreateNoWindow = true});
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}