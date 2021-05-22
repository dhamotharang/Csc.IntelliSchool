
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalClaimStatuses", Schema = "HR")]
  public partial class EmployeeMedicalClaimStatus {

    public EmployeeMedicalClaimStatus() {
      this.EmployeeMedicalClaims = new HashSet<EmployeeMedicalClaim>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int StatusID { get; set; }
    public string Name { get; set; }
    public Nullable<int> Order { get; set; }
    public string Notes { get; set; }
    public bool IsCompletion { get; set; }
    public bool IsPending { get; set; }
    public bool IsClaim { get; set; }


    public virtual ICollection<EmployeeMedicalClaim> EmployeeMedicalClaims { get; set; }
  }
}
