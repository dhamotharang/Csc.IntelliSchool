
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Csc.IntelliSchool.Data {

  [Table("EmployeeSalaries", Schema = "HR")]
  public partial class EmployeeSalary {
    [Key]
    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }


    public int Salary { get; set; }
    public int Taxes { get; set; }
    public int Social { get; set; }
    public int Medical { get; set; }

    public bool HideFromReports { get; set; }

  }
}
