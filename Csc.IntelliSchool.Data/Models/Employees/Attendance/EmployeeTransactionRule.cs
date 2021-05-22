
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeTransactionRules", Schema = "HR")]
  public partial class EmployeeTransactionRule {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RuleID { get; set; }
    public System.TimeSpan Time { get; set; }
    public decimal Points { get; set; }
    public string Type { get; set; }
  }
}
