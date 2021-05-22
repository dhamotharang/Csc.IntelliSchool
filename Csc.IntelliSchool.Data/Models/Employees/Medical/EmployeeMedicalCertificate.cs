
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeMedicalCertificates", Schema = "HR")]
  public partial class EmployeeMedicalCertificate {

    public EmployeeMedicalCertificate() {
      this.Dependants = new HashSet<EmployeeDependant>();
      this.Employees = new HashSet<Employee>();
    }


    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CertificateID { get; set; }
    public string Code { get; set; }
    public Nullable<System.DateTime> EnrollmentDate { get; set; }
    public Nullable<System.DateTime> CancellationDate { get; set; }
    public Nullable<int> Rate { get; set; }
    public Nullable<decimal> Concession { get; set; }
    public Nullable<int> MaximumConcession { get; set; }
    public Nullable<int> Monthly { get; set; }
    public string CancellationReason { get; set; }
    public string Owner { get; set; }
    public string RateType { get; set; }


    public virtual ICollection<EmployeeDependant> Dependants { get; set; }
    public virtual ICollection<Employee> Employees { get; set; }

    [ForeignKey("Program")]
    public int ProgramID { get; set; }
    public virtual EmployeeMedicalProgram Program { get; set; }

  }
}
