
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalProgramTemplates", Schema = "HR")]
  public partial class EmployeeMedicalProgramTemplate {
    [Key, Column(Order = 1)]
    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("Type")]
    public int TypeID { get; set; }
    public virtual EmployeeMedicalRequestType Type { get; set; }

    public string Path { get; set; }

  }
}
