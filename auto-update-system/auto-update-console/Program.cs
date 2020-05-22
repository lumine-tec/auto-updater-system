using System;
using System.IO;
using System.Threading;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

namespace auto_update_console
{
    class Program
    {
        const string VERSION = "v0.0.0.1";
        private static string bat = AppDomain.CurrentDomain.BaseDirectory + "updateApplication.bat";
        static void Main(string[] args)
        {
            var checkRelease = new Thread(CheckUpdate);
            checkRelease.Start();
            Console.WriteLine($"Versão atual: {VERSION}");
        }

        private static string GetInfoAPI()
        {
            try
            {
                var client = new RestClient("http://localhost:3000");
                var request = new RestRequest("/version", Method.GET);
                var response = client.Get(request);
                
                if (String.Compare(response.Content, VERSION) == 1)
                {
                    request = new RestRequest($"/version/{response.Content}", Method.GET);
                    var downloadData = client.DownloadData(request);

                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "newVersion.exe", downloadData);                    

                    return AppDomain.CurrentDomain.BaseDirectory + "newVersion.exe";
                }

                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
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
                    count++;

                    var returnOfAPI = GetInfoAPI();

                    if (returnOfAPI != "")
                    {
                        CreateBatFile(returnOfAPI);
                        Console.WriteLine("\n\nAtualização detectada! \nAperte \"Enter\" para atualizar");
                        Console.ReadLine();
                        ExecuteBat();
                    }

                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        private static void CreateBatFile(string fileName)
        {
            string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe";
            var command = $"taskkill /IM \"{appName}\" /F\r\n" +
                $"del /F \"auto-update-console.exe\"\r\n" +
                $"ren \"{fileName}\" \"{appName}\"\r\n" +
                $"start {appName}\r\n" +
                $"del /F \"{bat}\"";

            try
            {
                if (File.Exists(bat))
                    File.Delete(bat);

                File.WriteAllText(bat, command);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void ExecuteBat()
        {
            try
            {
                var process = new Process();
                process.StartInfo.FileName = bat;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
