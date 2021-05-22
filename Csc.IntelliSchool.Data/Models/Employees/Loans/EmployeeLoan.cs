
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeLoans", Schema = "HR")]
  public partial class EmployeeLoan {

    public EmployeeLoan() {
      this.Installments = new HashSet<EmployeeLoanInstallment>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LoanID { get; set; }
    public System.DateTime RequestDate { get; set; }
    public string Notes { get; set; }

    public DateTime StartMonth { get; set; }
    public DateTime EndMonth { get; set; }
    public int TotalAmount { get; set; }

    public virtual ICollection<EmployeeLoanInstallment> Installments { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
