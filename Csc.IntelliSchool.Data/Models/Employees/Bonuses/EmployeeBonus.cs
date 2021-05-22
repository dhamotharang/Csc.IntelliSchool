
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeBonuses", Schema = "HR")]
  public partial class EmployeeBonus {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BonusID { get; set; }
    public int EmployeeID { get; set; }
    public System.DateTime Date { get; set; }
    public Nullable<decimal> Points { get; set; }
    public Nullable<int> Value { get; set; }
    public bool IncludeInSalary { get; set; }
    public string Notes { get; set; }

    public virtual Employee Employee { get; set; }

    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeBonusType Type { get; set; }
  }
}
