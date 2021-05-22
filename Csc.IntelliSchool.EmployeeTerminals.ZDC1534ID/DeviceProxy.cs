using Csc.IntelliSchool.EmployeeTerminals.Common;
using Csc.IntelliSchool.EmployeeTerminals.ZDC1534ID.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Csc.IntelliSchool.EmployeeTerminals.ZDC1534ID {
  internal static class DeviceProxy {

    public static IEnumerable<TerminalLogEntry> GetLog(string ip, int port, int machineId, int pwd) {
      return CallDevice(ip, port, machineId, pwd, DeviceFunction.GetLog) as IEnumerable<TerminalLogEntry>;
    }


    public static void ClearLog(string ip, int port, int machineId, int pwd) {
      CallDevice(ip, port, machineId, pwd, DeviceFunction.ClearLog);
    }


    private static object CallDevice(string ip, int port, int machineId, int pwd, DeviceFunction func) {
      using (var dvc = new EmployeeTerminalDeviceReland(ip, port, machineId, pwd)) {
        object result = null;

        try {

          try {
            dvc.Initialize();
          } catch (Exception ex) {
            throw new Exception("Failed to initialize control", ex);
          }


          try {
            dvc.Connect();
          } catch (Exception ex) {
            throw new CommunicationException("Failed to connect to device", ex);
          }

          if (func == DeviceFunction.GetLog) {
            try {
              result = dvc.ReadLogEntries().ToArray();
            } catch (Exception ex) {
              throw new CommunicationException("Failed to read log entries", ex);
            }
          } else if (func == DeviceFunction.ClearLog) {
            try {
              dvc.ClearLogEntries();
            } catch (Exception ex) {
              throw new CommunicationException("Failed to clear log", ex);
            }
          }

        } finally {
          try {
            dvc.Disconnect();
          } catch {
          }
        }

        return result;
      }
    }
  }
}
