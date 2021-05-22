
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {
  [Table("EmployeeSocialInsurance", Schema = "HR")]
  public partial class EmployeeSocialInsurance {
    [Key]
    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    public string SocialID { get; set; }
    public System.DateTime EnrollmentDate { get; set; }
    public Nullable<System.DateTime> CancellationDate { get; set; }
    public int Basic { get; set; }
    public int Allowance { get; set; }
    public decimal Emergency { get; set; }
    public decimal Services { get; set; }

  }
}
