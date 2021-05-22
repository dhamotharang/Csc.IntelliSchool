
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeTerminals", Schema = "HR")]
  public partial class EmployeeTerminal {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TerminalID { get; set; }
    public string Name { get; set; }
    public string IP { get; set; }
    public Nullable<int> Port { get; set; }
    public Nullable<int> MachineID { get; set; }
    public Nullable<int> Password { get; set; }
    public string Model { get; set; }
  }
}
