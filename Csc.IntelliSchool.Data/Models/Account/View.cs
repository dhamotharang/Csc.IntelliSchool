using System.ComponentModel.DataAnnotations; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("Views")]
  public partial class View {

    public View() {
      this.ChildViews = new HashSet<View>();
      this.UserViews = new HashSet<UserView>();
    }

    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ViewID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Path { get; set; }
    public Nullable<int> Order { get; set; }
    public string Params { get; set; }
    public string Assembly { get; set; }


    public virtual ICollection<View> ChildViews { get; set; }

    public Nullable<int> ParentViewID { get; set; }
    public virtual View ParentView { get; set; }

    [ForeignKey("Group")]
    public Nullable<int> GroupID { get; set; }
    public virtual ViewGroup Group { get; set; }

    public virtual ICollection<UserView> UserViews { get; set; }
  }
}
