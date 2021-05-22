
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {


  [Table("EmployeeCustodies", Schema = "HR")]
  public partial class EmployeeCustody {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CustodyID { get; set; }

    public System.DateTime Date { get; set; }
    public int Qty { get; set; }
    public string Notes { get; set; }
    public Nullable<System.DateTime> RelinquishDate { get; set; }

    [ForeignKey("EmployeeCustodyItem")]
    public int ItemID { get; set; }
    public virtual EmployeeCustodyItem EmployeeCustodyItem { get; set; }


    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
