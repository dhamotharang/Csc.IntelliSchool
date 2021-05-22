
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeVacationTypes", Schema = "HR")]
  public partial class EmployeeVacationType {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeID{ get; set; }
    public string Name { get; set; }
    public bool IsPaid{ get; set; }
    public string Notes { get; set; }
  }
}
