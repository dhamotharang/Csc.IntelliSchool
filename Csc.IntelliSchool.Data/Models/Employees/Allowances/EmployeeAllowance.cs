
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeAllowances", Schema = "HR")]
  public partial class EmployeeAllowance {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AllowanceID { get; set; }

    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }


    public System.DateTime? StartMonth { get; set; }
    public System.DateTime? EndMonth { get; set; }

    public int Value { get; set; }

    public string Notes { get; set; }

    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeAllowanceType Type { get; set; }
  }
}
