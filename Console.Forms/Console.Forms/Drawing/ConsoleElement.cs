using System;
using System.Collections.Generic;
using System.Text;

namespace Crsouza.Console.Forms.Drawing
{
    public struct ConsoleElement
    {
        private char character;
        private ConsoleColor background;
        private ConsoleColor foreground;

        public ConsoleElement(char ch, ConsoleColor background, ConsoleColor foreground)
        {
            this.character = ch;
            this.background = background;
            this.foreground = foreground;
        }

        public char Character
        {
            get { return character; }
            set { character = value; }
        }


        public ConsoleColor Background
        {
            get { return background; }
            set { background = value; }
        }

        public ConsoleColor Foreground
        {
            get { return foreground; }
            set { foreground = value; }
        }

        public static readonly ConsoleElement Empty = new ConsoleElement('\0', ConsoleColor.Black, ConsoleColor.Black);

        public override bool Equals(object obj)
        {
            if (obj is ConsoleElement)
            {
                ConsoleElement conel = (ConsoleElement)obj;
                if (this.background == conel.Background &&
                    this.foreground == conel.foreground &&
                    this.character == conel.character)
                    return true;
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return character.GetHashCode() ^ background.GetHashCode() ^ foreground.GetHashCode();
        }

        public static bool operator ==(ConsoleElement a, ConsoleElement b)
        {
            return a.character == b.character &&
                a.background == b.background &&
                a.foreground == b.foreground;
        }

        public static bool operator !=(ConsoleElement a, ConsoleElement b)
        {
            return a.character != b.character ||
                a.background != b.background ||
                a.foreground != b.foreground; 
        }

    }
}
