
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {
  [Table("EmployeeTerminalTransactions", Schema = "HR")]
  public partial class EmployeeTerminalTransaction {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TransactionID { get; set; }
    public string TerminalIP { get; set; }
    public int UserID { get; set; }
    public System.DateTime DateTime { get; set; }
  }
}
