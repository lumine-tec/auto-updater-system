using System;
using System.Threading;

namespace auto_update_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var checkRelease = new Thread(checkUpdate);
            checkRelease.Start();
            Console.WriteLine("Main caindo fora.");
        }

        private static void checkUpdate()
        {
            var hour = 3600000;
            var minute = 60000;
            var count = 0;

            while (true)
            {
                Console.WriteLine(count + " minuto(s).");
                count++;

                Thread.Sleep(minute);
            }
        }
    }
}
