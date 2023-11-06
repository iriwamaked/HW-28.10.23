using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task6
{
    internal class Program
    {
        static Semaphore semaphore = new Semaphore(1,5);
        static void Main(string[] args)
        {
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread(GenerateRandomNumbers);
                thread.Start();
            }
            Console.ReadKey();

            //Task7
            bool isNew;
            Mutex mutex = new Mutex(true, "MyMutex", out isNew);

            if (isNew)
            {
                Console.WriteLine("Програма запущена.");
                Console.ReadLine(); 
                mutex.ReleaseMutex();
            }
            else
            {
                Console.WriteLine("Програма вже запущена. Неможливо запустити додаткові копії.");
                Thread.Sleep(2000); 
            }

        }

        static void GenerateRandomNumbers()
        {
            semaphore.WaitOne(); 

            try
            {
                Random random = new Random(Guid.NewGuid().GetHashCode());
                int threadId = Thread.CurrentThread.ManagedThreadId;

                Console.WriteLine($"Потік {threadId} виводить випадкові числа:\n");
                for (int i = 0; i < 5; i++)
                {
                    Console.Write($"{random.Next(1, 101)}  ");
                }
                Console.WriteLine();
            }
            finally
            {
                semaphore.Release(); 
            }
        }
    }
}
