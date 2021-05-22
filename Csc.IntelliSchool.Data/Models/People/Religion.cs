
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("Religions", Schema = "Ppl")]
  public partial class Religion {

    public Religion() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ReligionID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }

  }
}
