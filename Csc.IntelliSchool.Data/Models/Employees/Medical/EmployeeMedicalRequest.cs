
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalRequests", Schema = "HR")]
  public partial class EmployeeMedicalRequest {

    public EmployeeMedicalRequest() {
      this.Dependants = new HashSet<EmployeeMedicalRequestDependant>();
      this.Employees = new HashSet<EmployeeMedicalRequestEmployee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int RequestID { get; set; }
    public System.DateTime Date { get; set; }
    public string Status { get; set; }
    public string Notes { get; set; }



    public virtual ICollection<EmployeeMedicalRequestDependant> Dependants { get; set; }

    public virtual ICollection<EmployeeMedicalRequestEmployee> Employees { get; set; }

    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }


    [ForeignKey("Type")]
    public int TypeID { get; set; }
    public virtual EmployeeMedicalRequestType Type { get; set; }
  }
}
