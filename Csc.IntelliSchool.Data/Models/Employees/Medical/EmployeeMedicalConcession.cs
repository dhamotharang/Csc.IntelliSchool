
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalConcessions", Schema = "HR")]
  public partial class EmployeeMedicalConcession {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ConcessionID { get; set; }
    public int MinYears { get; set; }

    public decimal EmployeePercent { get; set; }
    public int? EmployeeMaximumAmount { get; set; }


    public decimal DependantPercent { get; set; }
    public int? DependantMaximumAmount { get; set; }


    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }
  }
}
