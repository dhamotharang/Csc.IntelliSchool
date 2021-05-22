
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {


  [Table("Nationalities", Schema = "Ppl")]
  public partial class Nationality {

    public Nationality() {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NationalityID { get; set; }
    public string Name { get; set; }
    public string ArabicName { get; set; }
    public bool IsLocal { get; set; }


  }
}
