// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Text;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Json
        {
            public static void Save<T>(T data, string name)
            {
                var path = GetJsonPath(name);
                var json = ToJson(data);
                File.WriteAllText(path, json);
            }

            public static void Load<T>(T data, string name)
            {
                var path = GetJsonPath(name);
                if (!File.Exists(path))
                {
                    Save(data, name);
                }

                var json = File.ReadAllText(path);
                FromJson(json, data);
            }

            public static void Encrypt<T>(T data, string name)
            {
                var path = GetJsonPath(name);
                var json = ToJson(data);
                json = Utility.Zip.Compress(json);
                var item = Encoding.UTF8.GetBytes(json);
                item = Utility.Xor.Encrypt(item);
                File.WriteAllBytes(path, item);
            }

            public static void Decrypt<T>(T data, string name)
            {
                var path = GetJsonPath(name);
                if (!File.Exists(path))
                {
                    Encrypt(data, name);
                }

                var item = File.ReadAllBytes(path);
                item = Utility.Xor.Decrypt(item);
                var json = Encoding.UTF8.GetString(item);
                json = Utility.Zip.Decompress(json);
                FromJson(json, data);
            }
        }
    }
}