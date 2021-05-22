
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDependants", Schema = "HR")]
  public partial class EmployeeDependant {

    public EmployeeDependant() {
      this.EmployeeMedicalRequestDependants = new HashSet<EmployeeMedicalRequestDependant>();
      this.EmployeeMedicalClaims = new HashSet<EmployeeMedicalClaim>();
      this.EmployeeMedicalProposalDependants = new HashSet<EmployeeMedicalProposalDependant>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DependantID { get; set; }



    [ForeignKey("MedicalCertificate")]
    public Nullable<int> MedicalCertificateID { get; set; }
    public virtual EmployeeMedicalCertificate MedicalCertificate { get; set; }
    public string Type { get; set; }


    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    [ForeignKey("Person")]
    public Nullable<int> PersonID { get; set; }
    public virtual Person Person { get; set; }

    public virtual ICollection<EmployeeMedicalRequestDependant> EmployeeMedicalRequestDependants { get; set; }

    public virtual ICollection<EmployeeMedicalClaim> EmployeeMedicalClaims { get; set; }

    public virtual ICollection<EmployeeMedicalProposalDependant> EmployeeMedicalProposalDependants { get; set; }
  }
}
