using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public class HumanResourcesFlagList : SystemFlagList {
 

    public TimeSpan? DuplicateTransactionTime {
      get {
        var val = GetValue(SystemFlagsModule.HumanResources, HumanResourcesFlagsSection.Attendance, ReflectionExtensions.GetPropertyName());
        return val != null ? TimeSpan.Parse(val.ToString()) : new TimeSpan?();
      }
    }

    public TimeSpan DefaultDuplicateTransactionTime {
      get {
        return new TimeSpan(0, 5, 0);
      }
    }
    public HumanResourcesFlagList(SystemFlag[] flags)
      : base(flags) {
    }
  }


}