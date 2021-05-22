using Csc.Components.Common.Data;
using System;
using System.ComponentModel;

namespace Csc.IntelliSchool.Data {
  public class EmployeeDataCriteria : RangeDataCriteria {
    public int[] EmployeeIDs { get; set; }
    public int[] ListIDs { get; set; }

    public int[] ItemIDs { get; set; }
    public int[] ItemTypeIDs { get; set; }


    public int[] BranchIDs { get; set; }
    public int[] DepartmentIDs { get; set; }
    public int[] PositionIDs { get; set; }
  }
}