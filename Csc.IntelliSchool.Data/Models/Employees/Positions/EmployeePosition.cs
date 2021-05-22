
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("EmployeePositions", Schema = "HR")]
  public partial class EmployeePosition {

    public EmployeePosition() {
      this.Lists = new HashSet<EmployeePositionList>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int PositionID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }


    public virtual ICollection<EmployeePositionList> Lists { get; set; }
  }
}
