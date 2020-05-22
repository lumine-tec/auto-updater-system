using System;
using System.IO;
using System.Threading;

namespace auto_update_console
{
    class Program
    {
        static void Main(string[] args)
        {
            var checkRelease = new Thread(CheckUpdate);
            checkRelease.Start();
            Console.WriteLine("Main caindo fora.");
        }

        private static string GetInfo()
        {
            try
            {

            }
        }

        private static void CheckUpdate()
        {
            var hour = 3600000;
            var minute = 60000;
            var count = 0;

            while (true)
            {
                try
                {
                    Console.WriteLine(count + " minuto(s).");
                    count++;

                    var returnOfAPI = GetInfo();

                    if (returnOfAPI != "")
                    {
                        CreateBatFile(returnOfAPI);

                        ExecuteBat();
                    }

                    Thread.Sleep(minute);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        private static void CreateBatFile(string fileName)
        {
            var bat = AppDomain.CurrentDomain.BaseDirectory + "updateApplication.bat";
            var command = $"taskkill \"auto - update - console.exe\" /IM /F\r\ndel / f \"auto-update-console.exe\"\r\nren \r\n\"{fileName}\" \"auto-update-console.exe\"\r\nauto - update - console.exe";
            try
            {
                if (File.Exists(bat))
                    File.Delete(bat);

                Console.WriteLine(bat);

                File.WriteAllText(bat, command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ExecuteBat()
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
