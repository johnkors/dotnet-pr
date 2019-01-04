namespace pr
{
    public class ColoredText
    {
        private int _color;
        private string _message;
        private bool _bright;

        public ColoredText(string message)
        {
            _message = message;
        }

        public ColoredText Bright()
        {
            _bright = true;
            return this;
        }

        public ColoredText Red()
        {
            _color = 31;
            return this;
        }

        public ColoredText Black()
        {
            _color = 30;
            return this;
        }

        public ColoredText Green()
        {
            _color = 32;
            return this;
        }

        public ColoredText Orange()
        {
            _color = 33;
            return this;
        }

        public ColoredText Blue()
        {
            _color = 34;
            return this;
        }

        public ColoredText Purple()
        {
            _color = 35;
            return this;
        }

        public ColoredText Cyan()
        {
            _color = 36;
            return this;
        }

        public ColoredText LightGray()
        {
            _color = 37;
            return this;
        }

        public static implicit operator string(ColoredText t)
        {
            return t.ToString();
        }

        public override string ToString()
        {
            if (StringExtensions.NoColor || _color == 0)
            {
                return _message;
            }

            string colorString = _color.ToString();
            if (_bright)
            {
                colorString += "m\x1B[1";
            }

            return $"\x1B[{colorString}m{_message}\x1B[0m\x1B[39m\x1B[49m";
        }
    }
}