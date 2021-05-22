
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeTerminal {

    [IgnoreDataMember]
    [NotMapped]
    public EmployeeTerminalModel? TerminalModel {
      get {
        if (string.IsNullOrEmpty(Model))
          return null;

        return (EmployeeTerminalModel)Enum.Parse(typeof(EmployeeTerminalModel), Model);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool CanFetch {
      get {
        return TerminalModel != null &&
          string.IsNullOrEmpty(IP) == false &&
          Port != null && Port != 0 &&
          MachineID != null &&
          Password != null;
      }
    }
  }


}