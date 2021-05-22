using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Windows.Forms.Integration;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Csc.IntelliSchool.Data {
  public abstract class EmployeeTerminalDevice {
    public virtual string IPAddress { get ;protected set; }
    public virtual int Port { get; protected set; }
    public virtual int MachineID { get; protected set; }
    public virtual int Password { get; protected set; }
    public bool Initialized { get; protected set; }
    public bool IsConnected { get; protected set; }
    public bool IsUpdating { get; protected set; }

    public AxHost AxHost { get; protected set; }
    public WindowsFormsHost Host { get; protected set; }

    #region Initialization
    public EmployeeTerminalDevice(string address, int port, int machineId, int pwd) {
      IPAddress = address;
      Port = port;
      MachineID = machineId;
      Password = pwd;
    }

    public virtual WindowsFormsHost Initialize() {
      this.Host = SyncCreateControl<WindowsFormsHost>();
      this.Host.Dispatcher.Invoke((Action)(() => { this.Host.Width = this.Host.Height = 0; this.Host.Child = AxHost; }));

      Initialized = true;
      return Host;
    }

    protected  static T SyncCreateControl<T>() where T : new() {
      return (T)Invoke(() =>  new T() );
    }
    protected static T Invoke<T>(Func<T> func) {
      return System.Windows.Application.Current.Dispatcher.Invoke(func);
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
      IsUpdating = enable == false;;
    }
    #endregion

    #region
    public virtual IEnumerable<EmployeeTerminalLogEntry> ReadLogEntries() {
      return new EmployeeTerminalLogEntry[] { };
    }

    public  virtual void ClearLogEntries() {

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
        throw new AxDeviceException(errCode, GetErrorMessage (errCode));
    }
    protected virtual string GetErrorMessage(int errCode) {
      return null;
    }
    #endregion
  }
}
