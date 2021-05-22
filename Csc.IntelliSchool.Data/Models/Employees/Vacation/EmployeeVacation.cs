
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {
  [Table("EmployeeVacations", Schema = "HR")]
  public partial class EmployeeVacation {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VacationID { get; set; }
    public System.DateTime StartDate { get; set; }
    public System.DateTime EndDate { get; set; }
    public string Notes { get; set; }
    public bool IsPaid { get; set; }


    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }



    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeVacationType Type { get; set; }
  }
}
