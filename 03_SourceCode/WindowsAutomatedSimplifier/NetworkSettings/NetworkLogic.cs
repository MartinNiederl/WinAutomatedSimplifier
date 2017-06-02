using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace WindowsAutomatedSimplifier.NetworkSettings
{

    internal static class NetworkLogic
    {
        public static List<Dictionary<string, object>> DictList { get; private set; }

        private static readonly NetworkInterface[] NetworkCards = NetworkInterface.GetAllNetworkInterfaces();

        public static List<KeyValuePair<string, object>> GetConCatNetworkCards()
        {
            var list = new List<KeyValuePair<string, object>>();

            foreach (NetworkInterface card in NetworkCards)
            {
                list.AddRange(typeof(NetworkInterface).GetProperties().Select(prop => new KeyValuePair<string, object>(prop.Name, prop.GetValue(card, null))));
                list.Add(new KeyValuePair<string, object>("", ""));
            }

            list.RemoveAt(list.Count - 1);
            return list;
        }

        public static IEnumerable<List<KeyValuePair<string, object>>> GetNetworkCardsProperties() => NetworkCards.Select(card => typeof(NetworkInterface).GetProperties()
            .Select(prop => new KeyValuePair<string, object>(prop.Name, prop.GetValue(card, null)))
            .ToList());

        public static List<string> GetNetworkCards() => NetworkCards.Select(card => $"{card.Name} ({card.Description})").ToList();


        public static List<KeyValuePair<string, object>> GetNetworkCardsByName(string name)
        {
            return NetworkCards.Where(c => c.Name.Equals(name))
                .Select(card => typeof(NetworkInterface).GetProperties()
                    .Select(prop => new KeyValuePair<string, object>(prop.Name, prop.GetValue(card, null)))
                    .ToList())
                .ElementAt(0);
        }

        public static void CreateNetInformation()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapter");

            DictList = (from ManagementObject adapter in searcher.Get()
                        select adapter.Properties.Cast<PropertyData>()
                        .Where(property => property.Value != null)
                        .ToDictionary(property => property.Name, property => property.Value))
                        .ToList();
        }

        public static Dictionary<string, object> GetNetInformationByName(string name)
        {
            var enumerable = DictList.Where(di => di["Name"].Equals(name));
            return !enumerable.Any() ? new Dictionary<string, object>() : enumerable.ElementAt(0);
        }

        public static List<object> GetNetInformationNames()
        {
            return DictList.Select(dict => dict["Name"]).ToList();
        }

        public static IPAddress GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
                if (ip.AddressFamily == AddressFamily.InterNetwork) return ip;
            throw new Exception("Local IP Address Not Found!");
        }

        public static IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (NetworkInterface adapter in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation unicastIPAddressInformation in adapter.GetIPProperties().UnicastAddresses)
                {
                    if (unicastIPAddressInformation.Address.AddressFamily != AddressFamily.InterNetwork) continue;
                    if (address.Equals(unicastIPAddressInformation.Address))
                        return unicastIPAddressInformation.IPv4Mask;
                }
            }
            throw new ArgumentException($"Can't find subnetmask for IP address '{address}'");
        }

        public static void RefreshIP()
        {
            ManagementClass objMC = new ManagementClass("Win32_NetworkAdapterConfiguration");

            foreach (ManagementObject objMO in objMC.GetInstances())
            {
                //Need to determine which adapter here with some kind of if() statement
                objMO.InvokeMethod("ReleaseDHCPLease", null, null);
                objMO.InvokeMethod("RenewDHCPLease", null, null);
            }
        }
    }
}
