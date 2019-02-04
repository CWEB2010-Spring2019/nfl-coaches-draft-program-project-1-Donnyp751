using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    class Program
    {
        static void enterPlayers(string _dataFilePath)
        {
            TextReader tReader = new StreamReader(_dataFilePath);
            string serializedData = tReader.ReadToEnd();
            tReader.Close();

            List<Player> players;// = new List<Player>();

            players = JsonConvert.DeserializeObject<List<Player>>(serializedData);

            ConsoleKey sentienniel = ConsoleKey.A;

            foreach (Player p in players)
            {
                Console.WriteLine(p);
            }

            do
            {
                Console.WriteLine("PlayerName, State, Position, Price, Pick");

                Player player = null;
                try
                {
                    player = new Player(Console.ReadLine(), Console.ReadLine(), Console.ReadLine(),
                        Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine()));

                }
                catch
                {
                    Console.WriteLine("invalid input");
                    continue;
                }

                Console.WriteLine(player);
                Console.WriteLine("Press up to confirm, down to reset");
                if (Console.ReadKey().Key == ConsoleKey.UpArrow)
                {
                    players.Add(player);
                    Console.WriteLine("Player Added: total players = " + players.Count.ToString() + "\n");
                }

                Console.WriteLine("press escape to quit, or any other key to continue adding players");
                sentienniel = Console.ReadKey().Key;
            } while (sentienniel != ConsoleKey.Escape);

            serializedData = JsonConvert.SerializeObject(players, Formatting.Indented);
            TextWriter tWriter = new StreamWriter(_dataFilePath);
            tWriter.Write(serializedData);

            tWriter.Close();
        }
        static void Main(string[] args)
        {
            string dataFilePath = @"..\..\..\playerDataFile.dat";

            if(args.Length > 0 && args[0] == "-addNew")
                enterPlayers(dataFilePath);





        }
    }


}
