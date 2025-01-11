// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 21:01:21
// # Recently: 2025-01-10 21:01:33
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;

namespace JFramework.Net
{
    internal static class Log
    {
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
        }

        public static void Warn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(Service.Text.Format("[{0}] {1}", DateTime.Now.ToString("MM-dd HH:mm:ss"), message));
        }
    }
}