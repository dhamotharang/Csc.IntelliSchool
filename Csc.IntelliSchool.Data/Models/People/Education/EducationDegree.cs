
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EducationDegrees", Schema = "Ppl")]
  public partial class EducationDegree {

    public EducationDegree() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int DegreeID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }
    public int? Order { get; set; }

  }
}
