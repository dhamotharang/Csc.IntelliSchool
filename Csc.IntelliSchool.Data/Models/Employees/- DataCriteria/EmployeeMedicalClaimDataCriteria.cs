using Csc.Components.Common.Data;
using System;
using System.ComponentModel;

namespace Csc.IntelliSchool.Data {
  public class EmployeeMedicalClaimDataCriteria  : RangeDataCriteria{
    public int[] ClaimIDs { get; set; }
    public int[] StatusIDs { get; set; }
  }
}