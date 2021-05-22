
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDepartmentVacations", Schema = "HR")]
  public partial class EmployeeDepartmentVacation {

    public EmployeeDepartmentVacation() {
      this.Departments = new HashSet<EmployeeDepartmentVacationLink>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VacationID { get; set; }
    public System.DateTime StartDate { get; set; }
    public System.DateTime EndDate { get; set; }
    public string Notes { get; set; }
    public string Name { get; set; }


    public virtual ICollection<EmployeeDepartmentVacationLink> Departments { get; set; }
  }
}
