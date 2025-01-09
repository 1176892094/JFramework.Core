// *********************************************************************************
// # Project: JFramework.Lobby
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-08-28 20:08:49
// # Recently: 2024-12-23 00:12:22
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.IO;
using System.Reflection;
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
            Transport transport = null;
            try
            {
                Debug.Log("运行服务器...");
                if (!File.Exists("setting.json"))
                {
                    var contents = JsonConvert.SerializeObject(new Setting(), Formatting.Indented);
                    await File.WriteAllTextAsync("setting.json", contents);

                    Debug.LogWarning("请将 setting.json 文件配置正确并重新运行。");
                    Console.ReadKey();
                    Environment.Exit(0);
                    return;
                }

                Setting = JsonConvert.DeserializeObject<Setting>(await File.ReadAllTextAsync("setting.json"));

                Debug.Log("加载程序集...");
                var assembly = Assembly.LoadFile(Path.GetFullPath(Setting.Assembly));

                Debug.Log("加载传输类...");
                transport = assembly.CreateInstance(Setting.Transport) as Transport;
                if (transport == null)
                {
                    Debug.LogError("没有找到传输类!");
                    Console.ReadKey();
                    Environment.Exit(0);
                    return;
                }

                transport.Awake();
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