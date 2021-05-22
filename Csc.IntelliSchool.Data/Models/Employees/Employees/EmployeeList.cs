
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;



namespace Csc.IntelliSchool.Data {

  [Table("EmployeeLists", Schema = "HR")]
  public partial class EmployeeList {

    public EmployeeList() {
      this.Employees = new HashSet<Employee>();
      this.Departments = new HashSet<EmployeeDepartmentList>();
      this.Positions = new HashSet<EmployeePositionList>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ListID { get; set; }
    public string Name { get; set; }


    public virtual ICollection<Employee> Employees { get; set; }

    public virtual ICollection<EmployeeDepartmentList> Departments { get; set; }

    public virtual ICollection<EmployeePositionList> Positions { get; set; }
  }
}
