using System;
using System.Net;

namespace UniP2P
{
    public static class IPEndPointParser
    {
        public static bool ComparisonAddressAndPort(IPEndPoint a, IPEndPoint b)
        {
            return a.Address.ToString() == b.Address.ToString() && a.Port == b.Port;
        }

        public static string ToString(IPEndPoint ip)
        {
            return string.Format("{0}:{1}", ip.Address, ip.Port);
        }

        public static IPEndPoint Parse(string ip)
        {
            var s = ip.Split(':');
            try
            {
                var end = new IPEndPoint(IPAddress.Parse(s[0]), int.Parse(s[1]));
                return end;
            }
            catch (Exception ex)
            {
                Debug.Debugger.Error(ex.Message);
                throw new Exception("[IPEndPointParser] Parse Exception");
            }
        }
    }
}
