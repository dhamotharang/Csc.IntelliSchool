
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalRequestEmployees", Schema = "HR")]
  public partial class EmployeeMedicalRequestEmployee {
    [Key, Column(Order = 1)]
    [ForeignKey("Request")]
    public int RequestID { get; set; }
    public virtual EmployeeMedicalRequest Request { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }


    public string CertificateCode { get; set; }

  }
}
