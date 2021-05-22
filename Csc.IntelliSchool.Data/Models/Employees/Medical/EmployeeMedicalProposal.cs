
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalProposals", Schema = "HR")]
  public partial class EmployeeMedicalProposal {

    public EmployeeMedicalProposal() {
      this.Dependants = new HashSet<EmployeeMedicalProposalDependant>();
      this.Employees = new HashSet<EmployeeMedicalProposalEmployee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProposalID { get; set; }
    public Nullable<System.DateTime> DateTimeUtc { get; set; }
    public string Name { get; set; }
    public string Notes { get; set; }


    public virtual ICollection<EmployeeMedicalProposalDependant> Dependants { get; set; }

    public virtual ICollection<EmployeeMedicalProposalEmployee> Employees { get; set; }
  }
}
