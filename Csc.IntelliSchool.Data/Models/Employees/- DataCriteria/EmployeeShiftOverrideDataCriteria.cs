using Csc.Components.Common;
using Csc.Components.Common.Data;
using System;
using System.ComponentModel;

namespace Csc.IntelliSchool.Data {
  public class EmployeeShiftOverrideDataCriteria : RangeDataCriteria {
    public int[] ShiftIDs { get; set; }
    public int[] ItemIDs { get; set; }
    public int[] TypeIDs { get; set; }
    public bool? Active { get; set; }
  }
}