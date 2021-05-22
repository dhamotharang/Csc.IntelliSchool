using Csc.IntelliSchool.EmployeeTerminals.Common;
using Riss.Devices;
using System;
using System.Linq;
using System.Collections.Generic;
using System.ServiceModel;

namespace Csc.IntelliSchool.EmployeeTerminals.ZDC2911 {

  internal static class DeviceProxy {
    #region State
    private static DateTime _startDate = DateTime.Today.AddYears(-1);
    private static DateTime _endDate = DateTime.Today.AddYears(1);
    private static List<DateTime> _dateRange = new List<DateTime>() { _startDate, _endDate };
    #endregion

    #region Public Interface
    public static IEnumerable<TerminalLogEntry> GetLog(string ip, int port, int id, string password) {
      return CallDevice(ip, port, id, password, DeviceFunction.GetLog) as IEnumerable<TerminalLogEntry>;
    }

    public static bool? ClearLog(string ip, int port, int id, string password) {
      return CallDevice(ip, port, id, password, DeviceFunction.GetLog) as bool?;
    }
    #endregion

    private static object CallDevice(string ip, int port, int id, string password, DeviceFunction func) {
      object result = null;
      Device dvc = CreateDevice(ip, port, id, password);
      var conn = DeviceConnection.CreateConnection(ref dvc);

      try {
        if (conn.Open() == 0) {
          throw new CommunicationException("Failed to open connection");
        }

        if (SetDeviceBusy(conn, dvc, true) == false) {
          throw new CommunicationException("Failed to set device busy");
        }


        if (func == DeviceFunction.GetLog) {
          result = GetRecords(conn, dvc).ToArray();
          if (result == null) {
            throw new CommunicationException("Failed to get records from device");
          }
        }else if (func == DeviceFunction.ClearLog) {
          var tmpResult = ClearLog(conn, dvc);
          if ((bool)result == false) {
            throw new CommunicationException("Failed to clear logs");
          }
        }

      } finally {
        try {
          SetDeviceBusy(conn, dvc, false);
        } catch { }
        try {
          conn.Close();
        } catch { }
      }

      return result;
    }

    private static Riss.Devices.Device CreateDevice(string ip, int port, int id, string password) {
      Riss.Devices.Device device = new Riss.Devices.Device();
      device.IpAddress = ip;
      device.IpPort = port;
      device.CommunicationType = Riss.Devices.CommunicationType.Tcp;
      device.DN = id; // device id
      device.Password = password; // comm key
      device.Model = "ZDC2911";
      device.ConnectionModel = 5;
      return device;
    }
    private static bool SetDeviceBusy(DeviceConnection conn, Device dvc, bool isBusy) {
      object extraProperty = new object();
      object extraData = (isBusy ? 1 : 0);

      return conn.SetProperty(DeviceProperty.Enable, extraProperty, dvc, extraData);
    }

    private static int GetRecordCount(DeviceConnection conn, Device dvc) {
      object extraProperty = false;
      object extraData = _dateRange;

      var result = conn.GetProperty(DeviceProperty.AttRecordsCount, extraProperty, ref dvc, ref extraData);
      if (result == false)
        return -1;

      return (int)extraData;
    }

    private static IEnumerable<TerminalLogEntry> GetRecords(DeviceConnection conn, Device dvc) {
      object extraProperty = new List<bool>() {
        false /* whole log */,
        false /* clear last log position */ };
      object extraData = _dateRange;

      var result = conn.GetProperty(DeviceProperty.AttRecords, extraProperty, ref dvc, ref extraData);

      if (result)
        foreach (var itm in (List<Record>)extraData) {
          yield return new TerminalLogEntry() {
            UserID = (int)itm.DIN,
            DateTime = itm.Clock
          };
        } 
    }


    private static bool ClearLog(DeviceConnection conn, Device dvc) {
      object extraProperty = new object();
      object extraData = new object();

      return conn.SetProperty(DeviceProperty.AttRecords, extraProperty, dvc, extraData);
    }
  }
}