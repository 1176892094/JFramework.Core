﻿using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JFramework.Net
{
    internal class Program
    {
        public static Setting Setting;
        public static Process Process;

        public static void Main(string[] args)
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public async Task MainAsync()
        {
            var transport = new Transport();
            try
            {
                Debug.Log("运行服务器...");
                if (!File.Exists("setting.json"))
                {
                    var contents = JsonConvert.SerializeObject(new Setting(), Formatting.Indented);
                    File.WriteAllText("setting.json", contents);

                    Debug.LogWarning("请将 setting.json 文件配置正确并重新运行。");
                    Console.ReadKey();
                    Environment.Exit(0);
                    return;
                }

                Setting = JsonConvert.DeserializeObject<Setting>(File.ReadAllText("setting.json"));

                Debug.Log("初始化传输类...");
                Process = new Process(transport);
                transport.OnServerError = Process.ServerError;
                transport.OnServerConnect = Process.ServerConnect;
                transport.OnServerReceive = Process.ServerReceive;
                transport.OnServerDisconnect = Process.ServerDisconnect;
                transport.port = Setting.RestPort;
                transport.StartServer();

                Debug.Log("开始进行传输...");
                if (Setting.UseEndPoint)
                {
                    Debug.Log("开启REST服务...");
                    if (!RestUtility.StartServer(Setting.RestPort))
                    {
                        Debug.LogError("请以管理员身份运行或检查端口是否被占用。");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                Console.ReadKey();
                Environment.Exit(0);
            }

            while (true)
            {
                transport.Update();
                await Task.Delay(Setting.UpdateTime);
            }
        }
    }
}