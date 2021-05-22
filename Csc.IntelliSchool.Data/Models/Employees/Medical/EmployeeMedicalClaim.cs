
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalClaims", Schema = "HR")]
  public partial class EmployeeMedicalClaim {

    public EmployeeMedicalClaim() {
      this.Documents = new HashSet<EmployeeMedicalClaimDocument>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ClaimID { get; set; }
    public string Reference { get; set; }
    public int Amount { get; set; }
    public Nullable<int> ClaimedAmount { get; set; }
    public string Notes { get; set; }
    public System.DateTime Date { get; set; }
    public Nullable<System.DateTime> CompletionDate { get; set; }


    public virtual ICollection<EmployeeMedicalClaimDocument> Documents { get; set; }

    [ForeignKey("Type")]
    public Nullable<int> TypeID { get; set; }
    public virtual EmployeeMedicalClaimType Type { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    [ForeignKey("Dependant")]
    public Nullable<int> DependantID { get; set; }
    public virtual EmployeeDependant Dependant { get; set; }

    [ForeignKey("Status")]
    public Nullable<int> StatusID { get; set; }
    public virtual EmployeeMedicalClaimStatus Status { get; set; }
  }
}
