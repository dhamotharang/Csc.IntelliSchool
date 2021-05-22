
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeLoanInstallments", Schema = "HR")]
  public partial class EmployeeLoanInstallment {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int InstallmentID { get; set; }
    public System.DateTime Month { get; set; }
    public int Amount { get; set; }


    [ForeignKey("Loan")]
    public int LoanID { get; set; }
    public virtual EmployeeLoan Loan { get; set; }
  }
}
