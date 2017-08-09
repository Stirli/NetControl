using NetControlCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace NetControlServer
{
    static class NetScanner
    {
        //https://docs.microsoft.com/en-us/dotnet/standard/parallel-programming/dataflow-task-parallel-library
        public static async Task ScanAllAsync(Action<IPAddress> callback, Action<IPAddress> report)
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
                throw new Exception("NetworkAvailable is not available");
            var interfaces = NetworkInterface.GetAllNetworkInterfaces();
            Ping pingSender = new Ping();
            pingSender.PingCompleted += (sender, args) =>
            {
                if (args.Reply.Status == IPStatus.Success) ;
                    //callback(args.Reply.Address);
            };
            TaskQueue tq = new TaskQueue(40);
            List<Task> tasks = new List<Task>();
            foreach (var ni in interfaces.Where(ni => ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                                      ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211))
            {
                foreach (UnicastIPAddressInformation ipinf in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ipinf.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {

                        var start = ipinf.Address.IpToInt() & ipinf.IPv4Mask.IpToInt();
                        var d = ipinf.IPv4Mask.IpToInt() ^ UInt32.MaxValue;
                        var end = start + d;
                        for (uint a = start, i = 0; a < end; a++, i++)
                        {
                            var ip = IntToIp(a);
                            tasks.Add(tq.Enqueue(() => pingSender.SendPingAsync(ip, 10, new byte[] { 1 }, new PingOptions(128, true))));
                        }
                    }
                }
            }
            await Task.WhenAll(tasks);
        }

        public static uint IpToInt(this IPAddress ip)
        {
            return BitConverter.ToUInt32(ip.GetAddressBytes().Reverse().ToArray(), 0);
        }

        private static IPAddress IntToIp(uint addr)
        {
            return new IPAddress(BitConverter.GetBytes(addr).Reverse().ToArray());
        }


    }
}
