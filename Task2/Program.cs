using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;

namespace Task2
{
    internal class Program
    {
        private static readonly object locker=new object();
        static void Main(string[] args)
        {
            string path = "text.txt";

            Thread firstThread = new Thread(() =>
            {
                Console.WriteLine($"Количество предложений: {CountSentences(path)}");
            });

            Thread secondThread = new Thread(() =>
            {
                ModifyFile(path, '#', '!');
                Console.WriteLine("Знаки успешно заменены");
            });
            firstThread.Start();
            secondThread.Start();

            firstThread.Join();
            secondThread.Join();
            Console.ReadKey();
        }

        static int CountSentences(string path) 
        {
            lock (locker) 
            { 
                string text=File.ReadAllText(path);
                string[] sentences = text.Split(new string[] { ". ", ", ", "? ", "! " }, StringSplitOptions.RemoveEmptyEntries);
                return sentences.Length;
            }
        }
        static void ModifyFile(string filePath, char oldChar, char newChar)
        {
            lock (locker)
            {
                string text = File.ReadAllText(filePath);
                text = text.Replace(oldChar, newChar);
                File.WriteAllText(filePath, text);
            }
        }
    }
}
