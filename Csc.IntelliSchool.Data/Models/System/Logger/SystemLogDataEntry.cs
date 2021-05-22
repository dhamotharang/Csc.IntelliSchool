using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class SystemLogDataEntry : SystemLogDataEntryBase {
    public int? EmployeeID { get; set; }
    public int[] EmployeeIDs { get; set; }
    public int? ParentID { get; set; }
    public int? Count { get; set; }
    public string EmployeeName { get; set; }
    public string Name { get; set; }
    public DateTime? Month { get; set; }
    public DateTime? Date { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public EmployeeRecalculateFlags EmployeeRecalculateFlags { get; set; }



    public SystemLogDataEntry() { }

    public SystemLogDataEntry(Employee emp) {
      EmployeeID = emp.EmployeeID;
      Name = emp.Person.FullName;
    }
  }


}