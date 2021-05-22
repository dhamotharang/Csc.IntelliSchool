
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalClaimDocuments", Schema = "HR")]
  public partial class EmployeeMedicalClaimDocument {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DocumentID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string OriginalFilename { get; set; }
    public Nullable<System.DateTime> LastUpdatedUtc { get; set; }

    [ForeignKey("Claim")]
    public int ClaimID { get; set; }
    public virtual EmployeeMedicalClaim Claim { get; set; }
  }
}
