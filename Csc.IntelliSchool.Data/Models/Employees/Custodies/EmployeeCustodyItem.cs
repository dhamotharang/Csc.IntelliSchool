using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeCustodyItems", Schema = "HR")]
  public partial class EmployeeCustodyItem {

    public EmployeeCustodyItem() {
      this.Item = new HashSet<EmployeeCustody>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ItemID { get; set; }
    public int Qty { get; set; }
    public string Name { get; set; }
    public string Status { get; set; }
    public string Description { get; set; }


    public virtual ICollection<EmployeeCustody> Item { get; set; }

    [ForeignKey("Employee")]
    public Nullable<int> EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
