using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("Users")]
  public partial class UserDependency {
    // If any of the following properties change, notification will be sent to the application

    public static readonly string TableName = "Users";

    public UserDependency() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int UserID { get; set; }

    public string Username { get; set; }
    public string Password { get; set; }
    public string PasswordFormat { get; set; }
    public string WindowsPrincipal { get; set; }
    public bool IsLocked { get; set; }

  }
}
