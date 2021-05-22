
using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public enum EmployeeTerminalDeviceErrorReland {
    Success = 0,
    ComPortError,
    WriteFail,
    ReadFail,
    InvalidParam,
    NonCarryout,
    LogEnd,
    MemoryError,
    MultiUserError,
    NoError
  }
}