
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDeductions", Schema = "HR")]
  public partial class EmployeeDeduction {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DeductionID { get; set; }
    public System.DateTime Date { get; set; }
    public Nullable<decimal> Points { get; set; }
    public Nullable<int> Value { get; set; }
    public string Notes { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }


    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeDeductionType Type { get; set; }
  }
}
