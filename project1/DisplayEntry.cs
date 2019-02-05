using System;
using System.Collections.Generic;
namespace project1
{
    class DisplayEntry
    {
        public struct ConsoleEntry
        {
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }
            public string Text;
        }

        private List<ConsoleEntry> Entries;

        public DisplayEntry()
        {
                Entries = new List<ConsoleEntry>();
        }

        public DisplayEntry(List<ConsoleEntry> consoleEntries)
        {
            Entries = consoleEntries;
        }
    }
}
