using System;
namespace Csc.IntelliSchool.EmployeeTerminals.ZDC1534ID.Device {

  [Serializable]
  public class AxDeviceException : Exception {
    public int ErrorCode { get; private set; }

    public AxDeviceException() { }
    public AxDeviceException(int errCode, string message) : base(message) { this.ErrorCode = errCode; }
    public AxDeviceException(string message) : base(message) { }
    public AxDeviceException(string message, Exception inner) : base(message, inner) { }
    protected AxDeviceException(
    System.Runtime.Serialization.SerializationInfo info,
    System.Runtime.Serialization.StreamingContext context)
      : base(info, context) { }
  }


}