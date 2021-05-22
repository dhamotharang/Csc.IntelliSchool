
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;



namespace Csc.IntelliSchool.Data {
  [Table("EmployeeMedicalProviders", Schema = "HR")]
  public partial class EmployeeMedicalProvider {

    public EmployeeMedicalProvider() {
      this.Programs = new HashSet<EmployeeMedicalProgram>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ProviderID { get; set; }
    public string Name { get; set; }


    public virtual ICollection<EmployeeMedicalProgram> Programs { get; set; }
  }
}
