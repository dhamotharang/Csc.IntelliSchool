
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalProposalEmployees", Schema = "HR")]
  public partial class EmployeeMedicalProposalEmployee {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordID { get; set; }
    public Nullable<decimal> Concession { get; set; }

    [ForeignKey("Program")]
    public Nullable<int> ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }

    [ForeignKey("Proposal")]
    public int ProposalID { get; set; }
    public virtual EmployeeMedicalProposal Proposal { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
