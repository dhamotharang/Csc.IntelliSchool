
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDepartmentVacationLinks", Schema = "HR")]
  public partial class EmployeeDepartmentVacationLink {
    [ForeignKey("Vacation")]
    [Key, Column(Order = 1)]
    public int VacationID { get; set; }
    public virtual EmployeeDepartmentVacation Vacation { get; set; }

    [ForeignKey("Department")]
    [Key, Column(Order = 2)]
    public int DepartmentID { get; set; }
    public virtual EmployeeDepartment Department { get; set; }

    public Nullable<bool> IgnoreThis { get; set; }
  }
}
