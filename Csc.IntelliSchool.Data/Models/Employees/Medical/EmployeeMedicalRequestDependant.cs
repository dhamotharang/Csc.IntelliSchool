
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalRequestDependants", Schema = "HR")]
  public partial class EmployeeMedicalRequestDependant {
    [Key, Column(Order = 1)]
    [ForeignKey("Request")]
    public int RequestID { get; set; }
    public virtual EmployeeMedicalRequest Request { get; set; }


    [Key, Column(Order = 2)]
    [ForeignKey("Dependant")]
    public int DependantID { get; set; }
    public virtual EmployeeDependant Dependant { get; set; }


    public string CertificateCode { get; set; }

  }
}
