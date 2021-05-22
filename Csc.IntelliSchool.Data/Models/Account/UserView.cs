
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Csc.IntelliSchool.Data {

  [Table("UserViews")]
  public partial class UserView {
    [Key, Column(Order = 1)]
    [ForeignKey("User")]
    public int UserID { get; set; }
    public virtual User User { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("View")]
    public int ViewID { get; set; }
    public virtual View View { get; set; }


    public Nullable<bool> IsHome { get; set; }
    public string Params { get; set; }

  }
}
