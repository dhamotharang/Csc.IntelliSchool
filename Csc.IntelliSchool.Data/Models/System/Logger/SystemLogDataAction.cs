
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public enum SystemLogDataAction {
    Insert,
    Update,
    Delete,
    Calculate,
    ApplyMedical,
    Terminate,
    Reenroll,
  }


}