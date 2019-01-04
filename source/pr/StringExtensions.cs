namespace pr
{
    public static class StringExtensions
    {
        public static bool NoColor { get; set; }

        public static ColoredText Orange(this string s)
        {
            return new ColoredText(s).Orange();
        }

        public static ColoredText Black(this string s)
        {
            return new ColoredText(s).Black();
        }

        public static ColoredText Red(this string s)
        {
            return new ColoredText(s).Red();
        }

        public static ColoredText Green(this string s)
        {
            return new ColoredText(s).Green();
        }

        public static ColoredText Blue(this string s)
        {
            return new ColoredText(s).Blue();
        }

        public static ColoredText Purple(this string s)
        {
            return new ColoredText(s).Purple();
        }

        public static ColoredText Cyan(this string s)
        {
            return new ColoredText(s).Cyan();
        }

        public static ColoredText LightGray(this string s)
        {
            return new ColoredText(s).LightGray();
        }
    }
}