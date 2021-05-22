
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class EmployeeDocument {
    [IgnoreDataMember]
    [NotMapped]
    public DateTime? LastUpdated { get { return LastUpdatedUtc != null ? LastUpdatedUtc.Value.ToLocalTime() : new DateTime?(); } }
  }

}