using System;

namespace pr
{
    internal static class Console
    {
        public static void OK(string s)
        {
            Write(s.Green());
        }
    
        public static void Info(string s)
        {
            Write(s.Blue());
        }

        public static void Warning(string s)
        {
            Write(s.Red());
        }

        private static void Write(string s)
        {
            System.Console.WriteLine(s);
        }
    }
}