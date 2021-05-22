
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeCharges", Schema = "HR")]
  public partial class EmployeeCharge {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ChargeID { get; set; }

    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }


    public System.DateTime? StartMonth { get; set; }
    public System.DateTime? EndMonth { get; set; }

    public int Value { get; set; }

    public string Notes { get; set; }

    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeChargeType Type { get; set; }
  }
}
