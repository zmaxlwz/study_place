using System;
using System.Diagnostics;
using System.Threading;

namespace ConsoleCpuRunningApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, the process starts...");
            var ranNum = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();
            while (true)
            {
                var upper = ranNum.Next(5000, 10000);
                var sum = 0;
                for (int i = 0; i < upper; i++)
                {
                    sum += i;
                }

                Console.WriteLine("upper: {0}, sum: {1}", upper, sum);

                if (watch.ElapsedMilliseconds > 1500)
                {
                    Thread.Sleep(2000);
                    watch.Reset();
                    watch.Start();
                }
            }
        }
    }
}
