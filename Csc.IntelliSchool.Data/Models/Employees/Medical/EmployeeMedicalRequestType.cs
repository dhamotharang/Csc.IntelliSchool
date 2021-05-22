
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalRequestTypes", Schema = "HR")]
  public partial class EmployeeMedicalRequestType {

    public EmployeeMedicalRequestType() {
      this.ProgramTemplates = new HashSet<EmployeeMedicalProgramTemplate>();
      this.Requests = new HashSet<EmployeeMedicalRequest>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeID { get; set; }
    public string Name { get; set; }


    public virtual ICollection<EmployeeMedicalProgramTemplate> ProgramTemplates { get; set; }

    public virtual ICollection<EmployeeMedicalRequest> Requests { get; set; }
  }
}
