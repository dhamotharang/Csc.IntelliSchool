using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeBankAccounts", Schema = "HR")]
  public partial class EmployeeBankAccount {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AccountID{ get; set; }


    public bool IsDefault { get; set; }
    public string AccountNumber { get; set; }
    public string RoutingNumber { get; set; }

    [ForeignKey("Bank")]
    public Nullable<int> BankID { get; set; }
    public virtual Bank Bank { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
