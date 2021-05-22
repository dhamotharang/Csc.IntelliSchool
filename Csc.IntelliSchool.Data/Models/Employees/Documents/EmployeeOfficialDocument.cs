
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeOfficialDocuments", Schema = "HR")]
  public partial class EmployeeOfficialDocument {
    [Key, Column(Order = 1)]
    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("Type")]
    public int TypeID { get; set; }
    public virtual EmployeeOfficialDocumentType Type { get; set; }


    public bool IsCompleted { get; set; }

  }
}
