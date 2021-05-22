
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalPrograms", Schema = "HR")]
  public partial class EmployeeMedicalProgram {

    public EmployeeMedicalProgram() {
      this.Concessions = new HashSet<EmployeeMedicalConcession>();
      this.Rates = new HashSet<EmployeeMedicalRate>();
      this.Certificates = new HashSet<EmployeeMedicalCertificate>();
      this.Templates = new HashSet<EmployeeMedicalProgramTemplate>();
      this.Requests = new HashSet<EmployeeMedicalRequest>();
      this.EmployeeMedicalProposalDependants = new HashSet<EmployeeMedicalProposalDependant>();
      this.EmployeeMedicalProposalEmployees = new HashSet<EmployeeMedicalProposalEmployee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProgramID { get; set; }
    public string Name { get; set; }
    public string PolicyCode { get; set; }
    public bool IsSystem { get; set; }

    [ForeignKey("Provider")]
    public int ProviderID { get; set; }
    public virtual EmployeeMedicalProvider Provider { get; set; }



    public virtual ICollection<EmployeeMedicalConcession> Concessions { get; set; }

    public virtual ICollection<EmployeeMedicalRate> Rates { get; set; }

    public virtual ICollection<EmployeeMedicalCertificate> Certificates { get; set; }

    public virtual ICollection<EmployeeMedicalProgramTemplate> Templates { get; set; }

    public virtual ICollection<EmployeeMedicalRequest> Requests { get; set; }
    public virtual EmployeeMedicalProgramInfo Info { get; set; }

    public virtual ICollection<EmployeeMedicalProposalDependant> EmployeeMedicalProposalDependants { get; set; }

    public virtual ICollection<EmployeeMedicalProposalEmployee> EmployeeMedicalProposalEmployees { get; set; }
  }
}
