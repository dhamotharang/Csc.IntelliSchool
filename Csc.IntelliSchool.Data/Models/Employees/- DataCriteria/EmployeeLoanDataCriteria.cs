using Csc.Components.Common;
using Csc.Components.Common.Data;
using System;
using System.ComponentModel;

namespace Csc.IntelliSchool.Data {
  public class EmployeeLoanDataCriteria : RangeDataCriteria {
    public int[] LoanIDs { get; set; }
    public int[] EmployeeIDs { get; set; }
    public int[] ListIDs { get; set; }
  }
}