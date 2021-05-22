using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using Csc.Components.Common;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {
  public abstract class EmployeeCalculateData {
    public Employee Employee { get; set; }
    public int EmployeeID { get { return Employee.EmployeeID; } }


    public EmployeeCalculateData() {

    }
    public EmployeeCalculateData(Employee emp) {
      this.Employee = emp;
    }
  }
}