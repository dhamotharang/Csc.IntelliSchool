
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EducationFields", Schema = "Ppl")]
  public partial class EducationField {

    public EducationField() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FieldID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }

  }
}
