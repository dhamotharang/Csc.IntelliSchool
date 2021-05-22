
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class EmployeeOfficialDocumentType {
    [IgnoreDataMember]
    [NotMapped]
    public string DisplayName {
      get {
        return string.IsNullOrEmpty(Abbreviation) ? Name : Abbreviation;
      }
    }
  }
}