
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("UserViews")]
  public partial class UserViewDependency {
    public static readonly string TableName = "UserViews";

    [Key, Column(Order = 1)]
    public int UserID { get; set; }

    [Key, Column(Order = 2)]
    public int ViewID { get; set; }
  }
}
