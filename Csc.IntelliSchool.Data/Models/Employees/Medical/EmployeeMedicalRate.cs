
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {


  [Table("EmployeeMedicalRates", Schema = "HR")]
  public partial class EmployeeMedicalRate {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RateID { get; set; }
    public int DependantValue { get; set; }
    public int MinAge { get; set; }
    public int MaxAge { get; set; }
    public int EmployeeValue { get; set; }
    public int CustomValue { get; set; }

    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }
  }
}
