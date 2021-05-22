using Csc.IntelliSchool.EmployeeTerminals.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows.Forms.Integration;

namespace Csc.IntelliSchool.EmployeeTerminals.ZDC1534ID.Device {
  public abstract class EmployeeTerminalDevice : IDisposable {
    public virtual string IPAddress { get; protected set; }
    public virtual int Port { get; protected set; }
    public virtual int MachineID { get; protected set; }
    public virtual int Password { get; protected set; }
    public bool Initialized { get; protected set; }
    public bool IsConnected { get; protected set; }
    public bool IsUpdating { get; protected set; }

    public AxHost AxHost { get; protected set; }
    public Form FormHost { get; protected set; }

    #region Initialization
    public EmployeeTerminalDevice(string address, int port, int machineId, int pwd) {
      IPAddress = address;
      Port = port;
      MachineID = machineId;
      Password = pwd;
    }

    public virtual void Initialize() {
      FormHost = new Form();
      FormHost.Visible = false;
      FormHost.Width = FormHost.Height = 0;
      FormHost.Controls.Add(AxHost);
      
      Initialized = true;
    }


    public void Dispose() {
      if (this.AxHost != null)
        this.AxHost.Dispose();
      if (this.FormHost != null)
        this.FormHost.Dispose();
    }

    #endregion

    #region Connect
    public virtual void Connect() {
      IsConnected = true;
    }

    public virtual void Disconnect() {
      IsConnected = false;
    }
    #endregion

    #region Enable/Disable
    /// <summary>
    /// Disables the device until EndUpdate is called
    /// </summary>
    protected void BeginUpdate() {
      EnableDevice(false);
    }
    /// <summary>
    /// Enable the device after BeginUpdate
    /// </summary>
    protected void EndUpdate() {
      EnableDevice(true);
    }
    protected virtual void EnableDevice(bool enable) {
      IsUpdating = enable == false; ;
    }
    #endregion

    #region
    public virtual IEnumerable<TerminalLogEntry> ReadLogEntries() {
      return null;
    }

    public virtual IEnumerable<TerminalUserEntry> ReadAllUsers() {
      return null;
    }

    public virtual void ClearLogEntries() {

    }
    #endregion

    #region Error Handling
    protected virtual int GetLastErrorCode() {
      int errCode = 0;
      return errCode;
    }
    protected void TryThrowException(bool hasError) { if (hasError) ThrowException(GetLastErrorCode()); }
    protected void ThrowException() { ThrowException(GetLastErrorCode()); }
    protected void ThrowException(int errCode) {
      if (errCode != 0)
        throw new AxDeviceException(errCode, GetErrorMessage(errCode));
    }
    protected virtual string GetErrorMessage(int errCode) {
      return null;
    }

    #endregion
  }
}
