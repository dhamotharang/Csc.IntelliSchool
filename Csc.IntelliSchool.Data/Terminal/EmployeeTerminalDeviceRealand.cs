using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using AxSBXPCLib;
using System.Windows.Forms.Integration;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public class EmployeeTerminalDeviceReland : EmployeeTerminalDevice {
    public AxSBXPC AxSbx { get { return this.AxHost as AxSBXPC; } set { this.AxHost = value; } }

    #region Initialization
    public EmployeeTerminalDeviceReland(string address, int port, int machineId, int pwd)
      : base(address, port, machineId, pwd) {
    }

    public override WindowsFormsHost Initialize() {
      this.AxSbx = SyncCreateControl<AxSBXPC>();
      this.AxSbx.CreateControl();

      return base.Initialize();
    }

    #endregion

    #region Connect / Disconnect
    public override void Connect() {
      string ipAddress = IPAddress;

      this.AxSbx.ReadMark = false;

      TryThrowException(
        this.AxSbx.ConnectTcpip(MachineID, ref ipAddress, Port, Password));

      BeginUpdate();

      base.Connect();
    }

    public override void Disconnect() {
      this.AxSbx.Disconnect();

      EndUpdate();

      base.Disconnect();
    }

    protected override void EnableDevice(bool enable) {
      TryThrowException(
        this.AxSbx.EnableDevice(MachineID, enable ? 1 : 0));

      base.EnableDevice(enable);
    }
    #endregion

    #region Reading
    public override IEnumerable<EmployeeTerminalLogEntry> ReadLogEntries() {
      bool retVal;

      retVal = this.AxSbx.ReadGeneralLogData(MachineID);
      if (GetLastError() == EmployeeTerminalDeviceErrorReland.LogEnd)
        yield break ;
      else
        ThrowException();

      while (true) {
        int tMachineNumber = 0;
        int enrollNumber = 0;
        int eMachineNumber = 0;
        int verifyMode = 0;
        int year = 0;
        int month = 0;
        int day = 0;
        int hour = 0;
        int minute = 0;
        int second = 0;

        retVal =
              this.AxSbx.GetGeneralLogData
              (MachineID,
              ref tMachineNumber,
              ref enrollNumber,
              ref eMachineNumber,
              ref verifyMode,
              ref year,
              ref month,
              ref day,
              ref hour,
              ref minute,
              ref second);

        if (retVal == false) {
          var error = GetLastError();
          if (error == EmployeeTerminalDeviceErrorReland.LogEnd)
            break;
          else
            ThrowException();
        } else {
          yield return new EmployeeTerminalLogEntry() {
            IP = this.IPAddress,
            UserID = enrollNumber,
            DateTime = new DateTime(year, month, day, hour, minute, second)
          };
        }
      }
    }
    public override void ClearLogEntries() {
      TryThrowException(this.AxSbx.EmptyGeneralLogData(MachineID));
    }

    #endregion

    #region Error Handling
    protected EmployeeTerminalDeviceErrorReland GetLastError() { return (EmployeeTerminalDeviceErrorReland)GetLastErrorCode(); }
    protected override int GetLastErrorCode() {
      int errCode = 0;
      this.AxSbx.GetLastError(ref errCode);
      return errCode;
    }

    protected override string GetErrorMessage(int errCode) {
      return ((EmployeeTerminalDeviceErrorReland)errCode).ToString();
    }
    #endregion
  }


}
