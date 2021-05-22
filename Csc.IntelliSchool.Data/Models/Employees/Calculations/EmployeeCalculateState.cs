using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Csc.Components.Common;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  /// <summary>
  /// Cumulative employee attendance/calculate operation state data.
  /// </summary>
  public abstract class EmployeeCalculateState {
    public DataEntities Model { get; set; }
    public HumanResourcesFlagList Flags { get; set; }


    public EmployeeCalculateState() {

    }
    public EmployeeCalculateState(DataEntities mdl) {
      this.Model = mdl;
    }
  }
}