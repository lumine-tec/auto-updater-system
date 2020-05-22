using System;
using System.IO;
using System.Threading;
using RestSharp;
using Newtonsoft.Json;

namespace auto_update_console
{
    class Program
    {
        private static string pathInfo = AppDomain.CurrentDomain.BaseDirectory + "info.json";
        private static object info;
        static void Main(string[] args)
        {
            info = GetInfoApplication();

            if(JsonConvert.SerializeObject(info) == "")
            {
                Console.WriteLine("Aplicação corrompida!");
                return;
            }

            var checkRelease = new Thread(CheckUpdate);
            checkRelease.Start();
            Console.WriteLine("Main caindo fora.");
        }

        private static object GetInfoApplication()
        {
            try
            {
                if (File.Exists(Program.pathInfo))
                {
                    return JsonConvert.DeserializeObject(File.ReadAllText(Program.pathInfo));
                }
                else
                {
                    return "";
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "";
            }
        }

        private static string GetInfoAPI()
        {
            try
            {
                var client = new RestClient("http://localhost:3000");

                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return "";
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

                    var returnOfAPI = GetInfoAPI();

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
