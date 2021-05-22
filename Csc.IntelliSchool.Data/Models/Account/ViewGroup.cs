using System.ComponentModel.DataAnnotations; using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("ViewGroups")]
  public partial class ViewGroup {

    public ViewGroup() {
      this.Views = new HashSet<View>();
    }

    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int GroupID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Nullable<int> Order { get; set; }
    public string Icon { get; set; }
    public string SmallIcon { get; set; }

    public virtual ICollection<View> Views { get; set; }
  }
}
