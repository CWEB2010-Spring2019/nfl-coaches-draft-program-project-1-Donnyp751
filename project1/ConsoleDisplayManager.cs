using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace project1
{
    class ConsoleDisplayManager
    {
        private int rows{ get; set; }
        private int columns { get; set; }
        private int placeHolderWidth = 20;
        private int placeHolderHeight = 4;
        private List<string> positions;
        private List<string> pickValues;

        public ConsoleDisplayManager(List<Player>players)
        {
            rows = 6;
            columns = 9;
            positions = new List<string>()
            {
                "Quarterback",
                "Running Back",
                "Wide-Receiver",
                "Defensive Lineman",
                "Defensive-Back",
                "Tight Ends",
                "Line-Backer's",
                "Offensive Tackles"
            };
            pickValues = new List<string>()
            {
                "1st",
                "2d",
                "3rd",
                "4th",
                "5th"
            };
        }

        private void WriteEmptyTable()
        {
            Console.SetWindowSize(placeHolderWidth * (rows) +1, (placeHolderHeight+2) * columns);
            Console.SetBufferSize(placeHolderWidth * (rows) +1, (placeHolderHeight+2)* columns);

            for (int i = 0; i <= columns; i++)
            {
                Console.SetCursorPosition(0, placeHolderHeight * i);
                for(int x = 0; x < placeHolderWidth * rows; x++)
                    Console.Write("-");
            }

            for (int i = 0; i < rows+1; i++)
            {
                int heightOffset = 1;
                Console.SetCursorPosition(i * placeHolderWidth, heightOffset);
                for (int x = 0; x < (placeHolderHeight+2) * (rows); x++)
                {
                    Console.Write("|");
                    Console.SetCursorPosition(i * placeHolderWidth, x+heightOffset);
                }
            }

        }

        public void WriteSingleEntry(List<string> text,Tuple<int,int> entryIndex, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black, int startingOffset=0)
        {
            int offset = 0;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            foreach (string t in text)
            {
                int len = t.Length;
                int bufferOffset = 0;
                if (!(len >= placeHolderWidth))
                {
                    bufferOffset = (placeHolderWidth - len) / 2;
                }

                Console.SetCursorPosition(entryIndex.Item1 + bufferOffset, entryIndex.Item2 + startingOffset + offset++);
                Console.Write(t);
            }
        }

        private void RunInteractiveTable()
        {

        }

        private void labelTable()
        {
            var test = new List<string>();
            test.Add("Positions");

            WriteSingleEntry(test, GetIndexPosition(0,0), ConsoleColor.Blue, startingOffset:1);

            foreach (string position in positions)
            {
                var printList = new List<string>();
                printList.Add(position);
                WriteSingleEntry(printList, GetIndexPosition(0, positions.IndexOf(position)+1), startingOffset:1);
            }

            foreach (string pick in pickValues)
            {
                var printList = new List<string>();
                printList.Add(pick);
                
                WriteSingleEntry(printList, GetIndexPosition(pickValues.IndexOf(pick)+1, 0), startingOffset:1);
            }
        }

        private Tuple<int, int> GetIndexPosition(int row, int column)
        {
            int rightDis = placeHolderWidth* row;
            int DownDis = placeHolderHeight * column;


            Tuple<int, int> pos =  new Tuple<int, int>(rightDis+1, DownDis+1);
            return pos;
        }
        public void Run()
        {
            Console.CursorVisible = false;
            WriteEmptyTable();
            labelTable();

            RunInteractiveTable();

        }

        

        

    }
}
