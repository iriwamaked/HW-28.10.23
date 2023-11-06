using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task3
{
    internal class Program
    {
        static Mutex mutex = new Mutex(false, "MyMutex");
        static string numbersFilePath = "numbers.txt";
        static string primeNumbersFilePath = "prime_numbers.txt";
        static string primeNumbersEndingWith7FilePath = "prime_numbers_ending_with_7.txt";
        static string records = "records.txt";
        static void Main()
        {
            Thread firstThread = new Thread(GenerateRandomNumbersAndWriteToFile);
            Thread secondThread = new Thread(ExtractPrimeNumbers);
            Thread thirdThread = new Thread(ExtractPrimeNumbersEndingWith7);
            Thread fourthThread = new Thread(Record);

            firstThread.Start();
            secondThread.Start();
            thirdThread.Start();
            fourthThread.Start();

            firstThread.Join();
            secondThread.Join();
            thirdThread.Join();
            fourthThread.Join();
        }

        static void GenerateRandomNumbersAndWriteToFile()
        {
            List<int> numbers = new List<int>();
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                int number = rand.Next(1, 1000);
                numbers.Add(number);
            }

            mutex.WaitOne();

            using (StreamWriter writer = new StreamWriter(numbersFilePath))
            {
                foreach (int number in numbers)
                {
                    writer.WriteLine(number);
                }
            }

            mutex.ReleaseMutex();
        }

        static void ExtractPrimeNumbers()
        {
            List<int> primes = new List<int>();

            mutex.WaitOne();

            using (StreamReader reader = new StreamReader(numbersFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (IsPrime(int.Parse(line)))
                    {
                        primes.Add(int.Parse(line));
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(primeNumbersFilePath))
            {
                foreach (int prime in primes)
                {
                    writer.WriteLine(prime);
                }
            }

            mutex.ReleaseMutex();
        }

        static void ExtractPrimeNumbersEndingWith7()
        {
            List<int> primesEndingWith7 = new List<int>();

            mutex.WaitOne();

            using (StreamReader reader = new StreamReader(primeNumbersFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    int number = int.Parse(line);
                    if (IsPrime(number) && number % 10 == 7)
                    {
                        primesEndingWith7.Add(number);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(primeNumbersEndingWith7FilePath))
            {
                foreach (int primeEndingWith7 in primesEndingWith7)
                {
                    writer.WriteLine(primeEndingWith7);
                }
            }

            mutex.ReleaseMutex();
        }
        static bool IsPrime(int number)
        {
            if (number < 2)
                return false;

            for (int i = 2; i <= Math.Sqrt(number); i++)
            {
                if (number % i == 0)
                    return false;
            }

            return true;
        }

        static void Record()
        {
            using (StreamWriter writer = new StreamWriter(records))
            {
                writer.WriteLine($"File: {numbersFilePath}");
                writer.WriteLine($"Количество чисел: {File.ReadAllLines(numbersFilePath).Length}");
                writer.WriteLine($"Размер в байтах: {new FileInfo(numbersFilePath).Length}");
                writer.WriteLine($"Содержимое:");
                writer.WriteLine(File.ReadAllText(numbersFilePath));
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine($"File: {primeNumbersFilePath}");
                writer.WriteLine($"Количество чисел: {File.ReadAllLines(primeNumbersFilePath).Length}");
                writer.WriteLine($"Размер в байтах: {new FileInfo(primeNumbersFilePath).Length}");
                writer.WriteLine($"Содержимое:");
                writer.WriteLine(File.ReadAllText(primeNumbersFilePath));
                writer.WriteLine();
                writer.WriteLine();
                writer.WriteLine($"File: {primeNumbersEndingWith7FilePath}");
                writer.WriteLine($"Количество чисел: {File.ReadAllLines(primeNumbersEndingWith7FilePath).Length}");
                writer.WriteLine($"Размер в байтах: {new FileInfo(primeNumbersEndingWith7FilePath).Length}");
                writer.WriteLine($"Содержимое:");
                writer.WriteLine(File.ReadAllText(primeNumbersEndingWith7FilePath));
                writer.WriteLine();
                writer.WriteLine();
            }
        }
    }

}
