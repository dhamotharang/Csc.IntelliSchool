using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace Csc.IntelliSchool.EmployeeTerminals.Common {
  public static class Helpers {

    public static string AppVersion {
      get {
        return System.Diagnostics.FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;
      }
    }

    public static string GetClientIP() {
      string ip = null;

      try { ip = GetLocalIP(); } catch { }
      if (ip == null) {
        try { ip = GetLocalIPAddresses(); } catch { }
      }

      return ip;
    }



    private static string GetLocalIP() {
      string localIP;
      using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0)) {
        socket.Connect("8.8.8.8", 65530);
        IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
        localIP = endPoint.Address.ToString();
      }
      return localIP;
    }

    private static string GetLocalIPAddresses() {
      if (!System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable()) {
        return null;
      }

      IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());

      return string.Join(",", host.AddressList.Where(s => s.AddressFamily == AddressFamily.InterNetwork).Select(s => s.ToString()));
    }
  }
}
