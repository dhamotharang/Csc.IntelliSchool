
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeePositionLists", Schema = "HR")]
  public partial class EmployeePositionList {
    [Key, Column(Order = 1)]
    [ForeignKey("Position")]
    public int PositionID { get; set; }
    public virtual EmployeePosition Position { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("List")]
    public int ListID { get; set; }
    public virtual EmployeeList List { get; set; }

    public Nullable<bool> IgnoreThis { get; set; }

  }
}
