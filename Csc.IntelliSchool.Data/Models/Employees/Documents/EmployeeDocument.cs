
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeDocuments", Schema = "HR")]
  public partial class EmployeeDocument {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DocumentID { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Url { get; set; }
    public string OriginalFilename { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Nullable<System.DateTime> LastUpdatedUtc { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }
  }
}
