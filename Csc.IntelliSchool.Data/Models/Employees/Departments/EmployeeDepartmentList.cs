
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {
  [Table("EmployeeDepartmentLists", Schema = "HR")]
  public partial class EmployeeDepartmentList {
    [Key, Column(Order = 1)]
    [ForeignKey("Department")]
    public int DepartmentID { get; set; }
    public virtual EmployeeDepartment Department { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("List")]
    public int ListID { get; set; }
    public virtual EmployeeList List { get; set; }


    public Nullable<bool> IgnoreThis { get; set; }

  }
}
