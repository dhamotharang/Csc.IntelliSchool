
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeSalaryUpdates", Schema = "HR")]
  public partial class EmployeeSalaryUpdate {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UpdateID { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public System.DateTime Date { get; set; }
    public int Salary { get; set; }
    public int Social { get; set; }
    public int Medical { get; set; }
    public int Taxes { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
