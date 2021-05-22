using Csc.IntelliSchool.EmployeeTerminals.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using zkemkeeper;

namespace Csc.IntelliSchool.EmployeeTerminals.ZKTecoK40 {
  internal static class DeviceProxy {

    #region Public Interface
    public static IEnumerable<TerminalLogEntry> GetLog(string ip, int port, int id) {
      return CallDevice(ip, port, id, DeviceFunction.GetLog) as IEnumerable<TerminalLogEntry>;
    }

    public static bool? ClearLog(string ip, int port, int id) {
      return CallDevice(ip, port, id, DeviceFunction.ClearLog) as bool?;
    }
    #endregion



    private static object CallDevice(string ip, int port, int id, DeviceFunction func) {
      object result = null;


      CZKEMClass device = new CZKEMClass();
      
      try {
        if (false == device.Connect_Net(ip, port)) {
          throw new CommunicationException("Failed to open connection");
        }

        if (device.EnableDevice(id, false) == false) {
          throw new CommunicationException("Failed to set disable device");
        }

        if (func == DeviceFunction.GetLog) {
          try {
            result = ReadLogEntries(device, id).ToArray();
          } catch (Exception ex) {
            throw new CommunicationException("Failed to read log entries", ex);
          }
        } else if (func == DeviceFunction.ClearLog) {
          try {
            ClearLogEntries(device, id);
          } catch (Exception ex) {
            throw new CommunicationException("Failed to clear log", ex);
          }
        }

      } finally {
        try {
          device.EnableDevice(id, true);
        } catch { }
        try {
          device.Disconnect();
        } catch { }
      }

      return result;
    }


    private static IEnumerable<TerminalLogEntry> ReadLogEntries(CZKEMClass device, int id) {
      if (false == device.ReadGeneralLogData(id)) {
        var lastError = GetLastError(device);
        if (lastError > 0)
          throw new CommunicationException("Error reading log data. Error Number: " + lastError.ToString());
        else
          yield break;
      }

      string sdwEnrollNumber = "";
      int idwVerifyMode = 0;
      int idwInOutMode = 0;
      int idwYear = 0;
      int idwMonth = 0;
      int idwDay = 0;
      int idwHour = 0;
      int idwMinute = 0;
      int idwSecond = 0;
      int idwWorkcode = 0;

      while (device.SSR_GetGeneralLogData(id, out sdwEnrollNumber, out idwVerifyMode,
                             out idwInOutMode, out idwYear, out idwMonth, out idwDay, out idwHour, out idwMinute, out idwSecond, ref idwWorkcode)) {

        yield return new TerminalLogEntry() {
          UserID = int.Parse(sdwEnrollNumber),
          DateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond)
        };
      }

    }


    private static void ClearLogEntries(CZKEMClass device, int id) {
      device.ClearGLog(id);
    }


    private static int GetLastError(CZKEMClass device) {
      int dwErrorCode = 0;
      device.GetLastError(ref dwErrorCode);
      return dwErrorCode;
    }
  }
}