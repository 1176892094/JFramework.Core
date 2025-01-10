// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-10 17:01:24
// # Recently: 2025-01-10 17:01:25
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace JFramework
{
    public struct PingUpdateEvent : IEvent
    {
        public double pingTime { get; private set; }

        public PingUpdateEvent(double pingTime)
        {
            this.pingTime = pingTime;
        }
    }

    public static partial class Service
    {
        public static class Address
        {
            public static string Host()
            {
                try
                {
                    var interfaces = NetworkInterface.GetAllNetworkInterfaces();
                    foreach (var inter in interfaces)
                    {
                        if (inter.OperationalStatus == OperationalStatus.Up && inter.NetworkInterfaceType != NetworkInterfaceType.Loopback)
                        {
                            var properties = inter.GetIPProperties();
                            foreach (var ip in properties.UnicastAddresses)
                            {
                                if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                                {
                                    return ip.Address.ToString();
                                }
                            }
                        }
                    }

                    var addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList; // 虚拟机无法解析网络接口 因此额外解析主机地址
                    foreach (var ip in addresses)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.ToString();
                        }
                    }

                    return IPAddress.Loopback.ToString();
                }
                catch
                {
                    return IPAddress.Loopback.ToString();
                }
            }
        }
    }
}