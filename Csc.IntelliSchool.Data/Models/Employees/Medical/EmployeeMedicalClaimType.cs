
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;



namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalClaimTypes", Schema = "HR")]
  public partial class EmployeeMedicalClaimType {

    public EmployeeMedicalClaimType() {
      this.Claims = new HashSet<EmployeeMedicalClaim>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeID { get; set; }
    public string Name { get; set; }


    public virtual ICollection<EmployeeMedicalClaim> Claims { get; set; }
  }
}
