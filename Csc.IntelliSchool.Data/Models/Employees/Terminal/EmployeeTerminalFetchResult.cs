
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public class EmployeeTerminalFetchResult {
    public string IP { get; set; }
    public int? EntryCount { get; set; }
    public int? UserCount { get; set; }
    public AggregateException Error { get; set; }

    [IgnoreDataMember]
    [NotMapped]
    public bool Success { get { return EntryCount != null && Error == null; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool SuccessWithErrors { get { return EntryCount != null && Error != null; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Failed { get { return EntryCount == null && Error != null; } }
  }

}