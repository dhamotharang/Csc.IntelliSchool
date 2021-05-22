
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public class SystemLogExceptionEntry : SystemLogDataEntryBase {
    public string Type { get; set; }
    public string Message { get; set; }
    public string StackTrace { get; set; }

    public string InnerType { get; set; }
    public string InnerMessage { get; set; }
    public string InnerStackTrace { get; set; }

    public SystemLogExceptionEntry(Exception ex) {
      Type = ex.GetType().FullName;
      Message = ex.Message;
      StackTrace = ex.StackTrace;

      if (ex.InnerException != null) {
        InnerType = ex.InnerException.GetType().FullName;
        InnerMessage = ex.InnerException.Message;
        InnerStackTrace = ex.InnerException.StackTrace;
      }
    }

  }

}