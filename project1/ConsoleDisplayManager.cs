using System;
using System.Collections.Generic;

namespace project1
{
    internal class ConsoleDisplayManager
    {
        private readonly int BudgetMax = 95000000;
        private readonly int CostEffBudgetMax = 65000000;
        private readonly int placeHolderHeight = 4;
        private readonly int placeHolderWidth = 20;
        private position currentPickIndex;
        private readonly Player[,] displayPlaceHolders;
        private readonly List<string> pickValues;
        private readonly List<Player> players;
        private readonly List<string> positions;
        private readonly List<Player> takenPlayers;

        public ConsoleDisplayManager(List<Player> players)
        {
            this.players = players;
            rows = 6;
            columns = 9;
            displayPlaceHolders = new Player[5, 8]; //5 wide 8 tall table for holding players + labels
            takenPlayers = new List<Player>();
            currentPickIndex = new position();
            positions = new List<string>
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
            pickValues = new List<string>
            {
                "1st",
                "2d",
                "3rd",
                "4th",
                "5th"
            };
        }

        private int rows { get; }
        private int columns { get; }

        private void WriteEmptyTable()
        {
            Console.SetWindowSize(placeHolderWidth * rows + 1, (placeHolderHeight + 2) * columns);
            Console.SetBufferSize(placeHolderWidth * rows + 1, (placeHolderHeight + 2) * columns);

            for (int i = 0; i <= columns; i++)
            {
                Console.SetCursorPosition(0, placeHolderHeight * i);
                for (int x = 0; x < placeHolderWidth * rows; x++)
                    Console.Write("-");
            }

            for (int i = 0; i < rows + 1; i++)
            {
                int heightOffset = 1;
                Console.SetCursorPosition(i * placeHolderWidth, heightOffset);
                for (int x = 0; x < (placeHolderHeight + 2) * rows; x++)
                {
                    Console.Write("|");
                    Console.SetCursorPosition(i * placeHolderWidth, x + heightOffset);
                }
            }

            WriteSingleEntry(
                new List<string>
                {
                    "Arrow keys to move selection", "Enter will toggle the selection of the player",
                    "Pressing Q or esc will close the program"
                }, GetIndexPosition(rows - 4, columns), ConsoleColor.White);
            WriteSingleEntry(new List<string> {"5 picks MAX", "95M total budget"}, GetIndexPosition(rows - 1, columns),
                ConsoleColor.White);
        }

        private void WriteSingleEntry(List<string> text, position entryIndex,
            ConsoleColor foregroundColor = ConsoleColor.White, ConsoleColor backgroundColor = ConsoleColor.Black,
            int startingOffset = 0)
        {
            int offset = 0;
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
            foreach (string t in text)
            {
                int len = t.Length;
                int bufferOffset = 0;
                if (!(len >= placeHolderWidth)) bufferOffset = (placeHolderWidth - len) / 2;

                Console.SetCursorPosition(entryIndex.x + bufferOffset, entryIndex.y + startingOffset + offset++);
                Console.Write(t);
            }
        }

        private void DisplayPlayers(position currentPosition)
        {
            for (int rowIndex = 0; rowIndex < rows - 1; rowIndex++)
            for (int colIndex = 0; colIndex < columns - 1; colIndex++)
            {
                Player player = displayPlaceHolders[rowIndex, colIndex];
                bool AlreadySelected = takenPlayers.Contains(player);
                ConsoleColor selectedColor = AlreadySelected ? ConsoleColor.DarkGreen : ConsoleColor.White;
                ConsoleColor indexedColor = ConsoleColor.Cyan;
                ConsoleColor color = new position(rowIndex, colIndex) == currentPosition ? indexedColor : selectedColor;

                WriteSingleEntry(player.getDisplayableList(), GetIndexPosition(rowIndex + 1, colIndex + 1), color);
            }
        }

        private void DisplayTakenPlayers(position currentIndex, bool highlight = false)
        {
            Player emptyPlayer = new Player();
            ConsoleColor color = ConsoleColor.Gray;
            for (int i = 1; i < rows; i++)
                WriteSingleEntry(emptyPlayer.getDisplayableList(), GetIndexPosition(i, columns + 1), color);
            foreach (Player player in takenPlayers)
            {
                var AlreadySelected = takenPlayers.Contains(player);

                color = ConsoleColor.Gray;

                WriteSingleEntry(player.getDisplayableList(),
                    GetIndexPosition(takenPlayers.IndexOf(player) + 1, columns + 1), color);
            }
        }

        private bool ToggleToTakenPlayers(position currentPosition)
        {
            if (takenPlayers.Contains(displayPlaceHolders[currentPosition.x, currentPosition.y]))
            {
                takenPlayers.Remove(displayPlaceHolders[currentPosition.x, currentPosition.y]);
                return true;
            }

            if (takenPlayers.Count >= 5) return false;
            foreach (Player player in takenPlayers)
                if (displayPlaceHolders[currentPosition.x, currentPosition.y] == player)
                    return false;
            if (GetPickedPlayersCost() + displayPlaceHolders[currentPosition.x, currentPosition.y].price < BudgetMax)
            {
                takenPlayers.Add(displayPlaceHolders[currentPosition.x, currentPosition.y]);
            }
            else
            {
                Console.Beep();
                Console.Beep();
            }

            return true;
        }

        private int GetPickedPlayersCost()
        {
            int totalCost = 0;
            foreach (Player player in takenPlayers) totalCost += player.price;

            return totalCost;
        }

        private void UpdatePickStats()
        {
            WriteSingleEntry(new Player().getDisplayableList(), GetIndexPosition(0, columns), ConsoleColor.White);

            byte flag = 0; //used for determining if there is at least one of the first, second, and third picks
            if (GetPickedPlayersCost() < CostEffBudgetMax)
                foreach (Player player in takenPlayers)
                    if (player.pick == 1)
                        flag |= 1;
                    else if (player.pick == 2)
                        flag |= 2;
                    else if (player.pick == 3)
                        flag |= 4;

            List<string> stats = new List<string>
            {
                "Cost " + GetPickedPlayersCost().ToString("C0"),
                "Remaining " + (BudgetMax - GetPickedPlayersCost()).ToString("C0"),
                flag == 7 ? "Cost Effective" : "                       "
            };
            WriteSingleEntry(stats, GetIndexPosition(0, columns), ConsoleColor.White);
        }

        private bool MoveWithBoundsEnforcement(position nextPosition, ref position currentPosition, position bounds,
            bool disableTakenBoundsEnforcement = true)
        {
            if (nextPosition.x < 0 || nextPosition.x > bounds.x - 2 || nextPosition.y < 0 ||
                nextPosition.y > bounds.y - 2)
            {
                Console.Beep();
                Console.Beep();
                //nextPosition = currentPosition;
                return false;
            }

            if (!disableTakenBoundsEnforcement
            ) //Legacy for jumping over taken player(s). I like the code so I don't wanna get rid of.
                foreach (Player player in takenPlayers)
                    if (displayPlaceHolders[nextPosition.x, nextPosition.y].Equals(player))
                    {
                        position currentInRecursivePosition = nextPosition;
                        if (nextPosition.x != currentPosition.x)
                        {
                            int changeVal = nextPosition.x - currentPosition.x > 0 ? 1 : -1;
                            nextPosition.x += changeVal;
                            return MoveWithBoundsEnforcement(nextPosition, ref currentPosition, bounds);
                        }

                        if (nextPosition.y != currentPosition.y)
                        {
                            int changeVal = nextPosition.y - currentPosition.y > 0 ? 1 : -1;
                            nextPosition.y += changeVal;
                            return MoveWithBoundsEnforcement(nextPosition, ref currentPosition, bounds);
                        }
                    }

            //if it reaches here then it should be a valid movement that doesn't end on a taken player
            currentPosition = nextPosition;
            return true;
        }


        private void RunInteractiveTable()
        {
            position nextIndex = currentPickIndex;
            position selectionBounds = new position(rows, columns);
            do
            {
                DisplayPlayers(currentPickIndex);
                DisplayTakenPlayers(currentPickIndex);
                UpdatePickStats();
                ConsoleKey key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.Enter:
                        ToggleToTakenPlayers(currentPickIndex);
                        break;

                    case ConsoleKey.DownArrow:
                        nextIndex.y++;
                        MoveWithBoundsEnforcement(nextIndex, ref currentPickIndex, selectionBounds);
                        break;

                    case ConsoleKey.UpArrow:
                        nextIndex.y--;
                        MoveWithBoundsEnforcement(nextIndex, ref currentPickIndex, selectionBounds);
                        break;
                    case ConsoleKey.LeftArrow:
                        nextIndex.x--;
                        MoveWithBoundsEnforcement(nextIndex, ref currentPickIndex, selectionBounds);
                        break;

                    case ConsoleKey.RightArrow:
                        nextIndex.x++;
                        MoveWithBoundsEnforcement(nextIndex, ref currentPickIndex, selectionBounds);
                        break;

                    //shutdown if either escape or q is hit
                    case ConsoleKey.Q:
                    case ConsoleKey.Escape:
                        return;

                    default:
                        continue;
                }

                nextIndex = currentPickIndex;
            } while (true);
        }

        private void sortPlayers()
        {
            foreach (Player player in players)
                displayPlaceHolders[player.pick - 1, positions.IndexOf(player.position)] = player;
        }

        private void labelTable()
        {
            var label = new List<string>();
            label.Add("Positions");

            WriteSingleEntry(label, GetIndexPosition(0, 0), ConsoleColor.Blue, startingOffset: 1);

            foreach (string position in positions)
            {
                var printList = new List<string>();
                printList.Add(position);
                WriteSingleEntry(printList, GetIndexPosition(0, positions.IndexOf(position) + 1), startingOffset: 1);
            }

            foreach (string pick in pickValues)
            {
                var printList = new List<string>();
                printList.Add(pick);

                WriteSingleEntry(printList, GetIndexPosition(pickValues.IndexOf(pick) + 1, 0), startingOffset: 1);
            }
        }

        private position GetIndexPosition(int row, int column)
        {
            int rightDis = placeHolderWidth * row;
            int DownDis = placeHolderHeight * column;

            position pos = new position(rightDis + 1, DownDis + 1);
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

        private struct position
        {
            public int x;
            public int y;

            public position(int _x = 0, int _y = 0)
            {
                x = _x;
                y = _y;
            }

            public static bool operator ==(position p1, position p2)
            {
                return IsEqual(p1, p2);
            }

            public static bool operator !=(position p1, position p2)
            {
                return !IsEqual(p1, p2);
            }

            private static bool IsEqual(position p1, position p2)
            {
                return p1.x == p2.x && p1.y == p2.y;
            }
        }
    }
}