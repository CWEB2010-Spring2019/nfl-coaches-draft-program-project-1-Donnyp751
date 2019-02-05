using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace project1
{
    class ConsoleDisplayManager
    {
        private struct position
        {
            public int x;
            public int y;

            public position(int _x = 0, int _y = 0)
            {
                x = _x;
                y = _y;
            }
        }

        private enum states
        {
            SelectingPicks,
            RemovingPicks,
            ReviewingPicks,
            Shutdown
        }

        private int rows{ get; set; }
        private int columns { get; set; }
        private int placeHolderWidth = 20;
        private int placeHolderHeight = 4;
        private List<string> positions;
        private List<string> pickValues;
        private List<Player> players;
        private List<Player> takenPlayers;
        private Player[,] displayPlaceHolders;

        public ConsoleDisplayManager(List<Player> players)
        {
            this.players = players;
            rows = 6;
            columns = 9;
            displayPlaceHolders = new Player[5,8]; //5 wide 8 tall table for holding players + labels
            takenPlayers = new List<Player>();
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

        private void WriteSingleEntry(List<string> text, position entryIndex, ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black, int startingOffset=0)
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

                Console.SetCursorPosition(entryIndex.x + bufferOffset, entryIndex.y + startingOffset + offset++);
                Console.Write(t);
            }
        }

        private void DisplayPlayers(position currentPosition)
        {
            for (int rowIndex = 0; rowIndex < rows-1; rowIndex++)
            {
                for (int colIndex = 0; colIndex < columns-1; colIndex++)
                {
                    Player player = displayPlaceHolders[rowIndex, colIndex];
                    bool AlreadySelected = takenPlayers.Contains(player);
                    ConsoleColor color = AlreadySelected ? ConsoleColor.DarkGreen : ConsoleColor.White;

                    WriteSingleEntry(player.getDisplayableList(), GetIndexPosition(rowIndex+1, colIndex+1), foregroundColor:color);

                }
            }

        }
        private states SelectingPicks()
        {
            position currentIndex = new position();
            position nextIndex = new position();
            ConsoleKey key;
            do
            {
                DisplayPlayers(currentIndex);
                key = Console.ReadKey().Key;
                switch (key)
                {
                    case ConsoleKey.Enter:

                        break;

                    case ConsoleKey.Backspace:

                        break;

                    case ConsoleKey.DownArrow:

                        break;

                    case ConsoleKey.UpArrow:

                        break;
                    case ConsoleKey.LeftArrow:

                        break;

                    case ConsoleKey.RightArrow:

                        break;

                    case ConsoleKey.Escape:
                        return states.Shutdown;

                    default:
                        continue;
                }


            } while (true);

        }
        private void RunInteractiveTable()
        {
            states state = states.SelectingPicks;

            bool running = true;
            do
            {
                switch (state)
                {
                    case states.SelectingPicks:
                        state = SelectingPicks();
                        break;
                    case states.RemovingPicks:
                        break;
                    case states.ReviewingPicks:
                        break;
                    case states.Shutdown:
                        running = false;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }




            } while (running);
        }

        private void sortPlayers()
        {
            foreach (Player player in players)
            {
                displayPlaceHolders[player.pick - 1, positions.IndexOf(player.position)] = player;
            }
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

        private position GetIndexPosition(int row, int column)
        {
            int rightDis = placeHolderWidth* row;
            int DownDis = placeHolderHeight * column;


            position pos =  new position(rightDis+1, DownDis+1);
            return pos;
        }
        public void Run()
        {
            Console.CursorVisible = false;
            WriteEmptyTable();
            labelTable();
            sortPlayers();

            RunInteractiveTable();
        }

        

        

    }
}
