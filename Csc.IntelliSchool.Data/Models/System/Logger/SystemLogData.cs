
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("SystemLogData", Schema = "App")]
  public partial class SystemLogData {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DataID { get; set; }
    public string Property { get; set; }
    public string Value { get; set; }

    [ForeignKey("Log")]
    public int LogID { get; set; }
    public virtual SystemLog Log { get; set; }
  }
}
