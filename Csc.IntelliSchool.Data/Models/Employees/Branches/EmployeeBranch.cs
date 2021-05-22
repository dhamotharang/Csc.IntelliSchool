
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeBranches", Schema = "HR")]
  public partial class EmployeeBranch {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BranchID { get; set; }

    public string Name { get; set; }
    public string ArabicName { get; set; }
  }
}
