
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum UserDataFilter {
    None = 0,
    UserViews = 1 << 0,  // Religion, Nationality
    Views = 1 << 1,
  }


}