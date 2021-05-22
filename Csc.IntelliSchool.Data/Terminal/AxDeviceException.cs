using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

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