using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class IpFinder
    {
        public static string GetIp()
        {
            NetworkInterface[] networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

            foreach (var networkInterface in networkInterfaces)
            {
                // Check if the interface is up and not a loopback or tunnel interface
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                    networkInterface.NetworkInterfaceType != NetworkInterfaceType.Tunnel)
                {
                    // Get the IP properties of the interface
                    IPInterfaceProperties ipProperties = networkInterface.GetIPProperties();

                    // Get the unicast addresses (IP addresses) assigned to the interface
                    foreach (var ipAddress in ipProperties.UnicastAddresses)
                    {
                        // Only consider IPv4 addresses
                        if (ipAddress.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            return ipAddress.Address.ToString();
                        }
                    }
                }
            }

            return null;
        }
    }
}
