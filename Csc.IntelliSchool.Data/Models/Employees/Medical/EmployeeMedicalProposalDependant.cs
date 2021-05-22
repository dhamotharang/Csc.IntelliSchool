
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalProposalDependants", Schema = "HR")]
  public partial class EmployeeMedicalProposalDependant {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RecordID { get; set; }
    public Nullable<decimal> Concession { get; set; }

    [ForeignKey("Dependant")]
    public int DependantID { get; set; }
    public virtual EmployeeDependant Dependant { get; set; }

    [ForeignKey("Program")]
    public Nullable<int> ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }

    [ForeignKey("Proposal")]
    public int ProposalID { get; set; }
    public virtual EmployeeMedicalProposal Proposal { get; set; }
  }
}
