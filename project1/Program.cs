using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace project1
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataFilePath = @"..\..\..\playerDataFile.dat";

            Player p1 = new Player("Dwayne Haskins","Ohio St.", "Quarterback" , 24600100, 1 );
            Player p2 = new Player("Josh Jacobs", "Alabama", "Quarterback", 24500100, 1);
            List<Player> players = new List<Player>();

            TextReader tReader = new StreamReader(dataFilePath);
            string serializedData = tReader.ReadToEnd();
            tReader.Close();

            players.Add(p1);
            players.Add(p2);



            //players = JsonConvert.DeserializeObject<List<Player>>(serializedData);

            ConsoleKey sentienniel = ConsoleKey.A;

            foreach (Player p in players)
            {
                Console.WriteLine(p);
            }

            while (sentienniel != ConsoleKey.Escape)
            {
                Console.WriteLine("PlayerName, State, Position, Price, Pick");
                var player = new Player(Console.ReadLine(), Console.ReadLine(), Console.ReadLine(), Convert.ToInt32(Console.ReadLine()), Convert.ToInt32(Console.ReadLine())) ;
                Console.Write(player);
                Console.WriteLine("Press up to confirm, down to reset");
                if (Console.ReadKey().Key == ConsoleKey.UpArrow)
                {
                    players.Add(player);
                    Console.WriteLine("Player Added: total players =" + players.Count.ToString() );
                }
                Console.WriteLine("press escape to quit, or any other key to continue adding players");
                sentienniel = Console.ReadKey().Key;
            }

            serializedData = JsonConvert.SerializeObject(players, Formatting.Indented);
            TextWriter tWriter = new StreamWriter(dataFilePath);
            tWriter.Write(serializedData);

            tWriter.Close();

            Console.ReadKey();


        }
    }


}
