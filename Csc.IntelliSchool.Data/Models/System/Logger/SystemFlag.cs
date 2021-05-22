
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Csc.IntelliSchool.Data {

  [Table("SystemFlags", Schema = "App")]
  public partial class SystemFlag {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FlagID { get; set; }
    public string Module { get; set; }
    public string Section { get; set; }
    public string Name { get; set; }
    public string Value { get; set; }
    public string Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
  }
}
