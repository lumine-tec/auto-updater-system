using System;
using System.IO;
using System.Threading;
using RestSharp;
using Newtonsoft.Json;
using System.Diagnostics;

namespace auto_update_console
{
    class Program
    {
        private static string pathInfo = AppDomain.CurrentDomain.BaseDirectory + "info.json";
        private static dynamic info;
        private static string bat = AppDomain.CurrentDomain.BaseDirectory + "updateApplication.bat";
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

        private static dynamic GetInfoApplication()
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
                var request = new RestRequest("/version", Method.GET);
                var response = client.Get(request);
                
                if (String.Compare(response.Content, Convert.ToString(info.version)) == 1)
                {
                    request = new RestRequest("/version/", Method.GET);
                    var downloadData = client.DownloadData(request);

                    File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "newVersion.exe", downloadData);

                    info.version = response.Content;
                    UpdateJSON();

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

        private static void UpdateJSON()
        {
            try
            {
                File.WriteAllText(pathInfo, JsonConvert.SerializeObject(info));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
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

                    Thread.Sleep(hour);
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
            var command = $"taskkill /IM \"{appName}\" /F\r\ndel /F \"auto-update-console.exe\"\r\nren \"{fileName}\" \"auto-update-console.exe\"\r\nauto-update-console.exe\r\ndel /F \"updateApplication.bat\"";
            try
            {
                if (File.Exists(bat))
                    File.Delete(bat);

                Console.WriteLine(bat);

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
                Console.WriteLine("Executando bat");
                process.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
