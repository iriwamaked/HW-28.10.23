using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace HW_28._10._23
{
    internal class Program
    {
        static ManualResetEvent generationComplite = new ManualResetEvent(false);
        static ManualResetEvent sumComplete = new ManualResetEvent(false);
        static ManualResetEvent multyComplete = new ManualResetEvent(false);
        static List<string> pairs = new List<string>();
        
        static void Main(string[] args)
        {
            Thread gener = new Thread(GenerateNums);
            Thread sumThread = new Thread(SumsOfPairs);
            Thread multypThread = new Thread(MultipOfPairs);

            gener.Start();
            sumThread.Start();
            multypThread.Start();

            WaitHandle.WaitAll(new[] {sumComplete, multyComplete});
            Console.WriteLine("Sum and multy calculation completed.");
        }

        static void GenerateNums()
        {
            Random rand = new Random();
            string filePath = "pairs.txt";
            using (StreamWriter sw = File.CreateText(filePath))
            {
                for (int i = 0; i < 10; i++)
                {
                    int firstNumber = rand.Next(1, 100);
                    int secondNumber = rand.Next(1, 100);
                    pairs.Add($"{firstNumber},{secondNumber}");
                    string line = $"{firstNumber},{secondNumber}";
                    sw.WriteLine(line);
                    Console.WriteLine($"{i} пара: {firstNumber}, { secondNumber}\n") ;
                }
            }
            generationComplite.Set();
        }

        static void SumsOfPairs()
        {
            generationComplite.WaitOne();
            List<int> sums = new List<int>();
            Console.WriteLine("\t\nCуммы сгенерированных пар:");
            foreach (string el in  pairs) 
            {
                string[] numbers = el.Split(',');
                int firstNumber = int.Parse(numbers[0]);
                int secondNumber = int.Parse(numbers[1]);
                int sum = firstNumber + secondNumber;
                sums.Add(sum);
                Console.WriteLine($"{firstNumber} + {secondNumber} = {sum}") ;
            }
            List<string> stringSums = sums.Select(s => s.ToString()).ToList();
            File.WriteAllLines("sums.txt", stringSums);
            sumComplete.Set();
        }

        static void MultipOfPairs()
        {
            generationComplite.WaitOne();
            List<int> multy = new List<int>();
            Console.WriteLine("\n\tРезультат умножения:\n");
            foreach (string el in pairs)
            {
                string[] numbers = el.Split(',');
                int firstNumber = int.Parse(numbers[0]);
                int secondNumber = int.Parse(numbers[1]);
                int multyply = firstNumber * secondNumber;
                multy.Add(multyply);
                Console.WriteLine($"{firstNumber} * {secondNumber} = {multyply}");
            }
            List<string> stringSums = multy.Select(s => s.ToString()).ToList();
            File.WriteAllLines("multy.txt", stringSums);
            multyComplete.Set();
        }
    }
}
