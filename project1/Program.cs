﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    internal class Program
    {
        //This function was made to speed up the entry of the data from the image we were given into json.
        private static void enterPlayers(string _dataFilePath)
        {
            TextReader tReader = new StreamReader(_dataFilePath);
            string serializedData = tReader.ReadToEnd();
            tReader.Close();

            List<Player> players; // = new List<Player>();

            players = JsonConvert.DeserializeObject<List<Player>>(serializedData);

            ConsoleKey sentienniel = ConsoleKey.A;

            foreach (Player p in players) Console.WriteLine(p);

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
                    Console.WriteLine("Player Added: total players = " + players.Count + "\n");
                }

                Console.WriteLine("press escape to quit, or any other key to continue adding players");
                sentienniel = Console.ReadKey().Key;
            } while (sentienniel != ConsoleKey.Escape);

            serializedData = JsonConvert.SerializeObject(players, Formatting.Indented);
            TextWriter tWriter = new StreamWriter(_dataFilePath);
            tWriter.Write(serializedData);

            tWriter.Close();
        }

        private static List<Player> readPlayersFile(string path)
        {
            using (TextReader tReader = new StreamReader(path))
            {
                string serializedData = tReader.ReadToEnd();
                return
                    JsonConvert.DeserializeObject<List<Player>>(
                        serializedData); //return the list of player objects from the file
            }
        }

        private static void Main(string[] args)
        {
            string dataFilePath = @"..\..\..\playerDataFile.dat";

            if (args.Length > 0 && args[0] == "-addNew")
                enterPlayers(dataFilePath);

            List<Player> players = readPlayersFile(dataFilePath);

            ConsoleDisplayManager consoleManager = new ConsoleDisplayManager(players);
            consoleManager.Run();
        }
    }
}