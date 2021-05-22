
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {


    [Table("Users")]
  public partial class User {

    public User() {
      this.UserViews = new HashSet<UserView>();
    }

    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordFormat { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsLocked { get; set; }
    public string WindowsPrincipal { get; set; }
    public bool CanChangePassword { get; set; }


    public virtual ICollection<UserView> UserViews { get; set; }
  }
}
