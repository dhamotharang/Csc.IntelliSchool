
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("Banks", Schema = "Ppl")]
  public partial class Bank {

    public Bank() {
      this.EmployeeBankAccounts = new HashSet<EmployeeBankAccount>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BankID { get; set; }
    public string Name { get; set; }
    public string Branch { get; set; }


    public virtual ICollection<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
  }
}
