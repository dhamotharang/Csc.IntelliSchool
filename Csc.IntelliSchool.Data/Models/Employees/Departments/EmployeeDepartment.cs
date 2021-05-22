
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDepartments", Schema = "HR")]
  public partial class EmployeeDepartment {

    public EmployeeDepartment() {
      this.Lists = new HashSet<EmployeeDepartmentList>();
      this.EmployeeDepartmentOfficialDocuments = new HashSet<EmployeeDepartmentOfficialDocument>();
      this.EmployeeDepartmentVacationLinks = new HashSet<EmployeeDepartmentVacationLink>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DepartmentID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }


    public virtual ICollection<EmployeeDepartmentList> Lists { get; set; }

    public virtual ICollection<EmployeeDepartmentOfficialDocument> EmployeeDepartmentOfficialDocuments { get; set; }

    public virtual ICollection<EmployeeDepartmentVacationLink> EmployeeDepartmentVacationLinks { get; set; }
  }
}
