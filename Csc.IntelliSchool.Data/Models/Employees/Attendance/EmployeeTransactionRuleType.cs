
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public enum EmployeeTransactionRuleType {
    Unknown = 0,
    In,
    Out,
    TimeOff,
    Overtime
  }

}