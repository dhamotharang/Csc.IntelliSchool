
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeChargeTypes", Schema = "HR")]
  public partial class EmployeeChargeType {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeID{ get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }
  }
}
