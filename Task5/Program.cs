using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task5
{
    internal class Program
    {
        internal class Player
        {
            public int InitialAmount { get; set; }
            public int CurrentAmount { get; set; }
        }
        
        private static Semaphore tableSemaphore = new Semaphore(1,5); // Дозволяє одночасно грати 5 гравцям
        private static object fileLock = new object();
        private static List<Player> players = new List<Player>();

        static void Main()
        {
            int totalPlayers = new Random().Next(20, 30); // Випадкова кількість гравців

            for (int i = 0; i < totalPlayers; i++)
            {
                players.Add(new Player { InitialAmount = 1000, CurrentAmount=1000 }); // Кожний гравець має початкову суму 1000
            }

            RunDay();
            Console.ReadKey();
        }

        static void RunDay()
        {
            using (StreamWriter writer = new StreamWriter("report.txt"))
            {
                foreach (var player in players)
                {
                    tableSemaphore.WaitOne(); // Очікуємо доступу до столу
                    PlayRoulette(player);
                    tableSemaphore.Release(); // Звільняємо доступ до столу

                    lock (fileLock)
                    {
                        writer.WriteLine($"Гравець {players.IndexOf(player) + 1} [{player.InitialAmount}] [{player.CurrentAmount}]");
                    }
                }
            }
        }

        static ThreadLocal<Random> localRandom = new ThreadLocal<Random>(() => new Random());
        static void PlayRoulette(Player player)
        {
            int bet = localRandom.Value.Next(1, 37);
            int amount = localRandom.Value.Next(1, player.CurrentAmount + 1);

            bool win = localRandom.Value.Next(0, 37) == bet;

            if (win)
            {
                player.CurrentAmount += amount;
            }
            else
            {
                player.CurrentAmount -= amount;
            }
        }


    }
}
