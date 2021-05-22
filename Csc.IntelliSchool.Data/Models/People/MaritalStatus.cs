
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public enum MaritalStatus {
    Unknown = 0,
    Single,
    Married,
    Divorced,
    Widowed
  }

}