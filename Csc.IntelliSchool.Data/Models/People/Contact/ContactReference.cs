
using System.ComponentModel.DataAnnotations; using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {

[Table("ContactReferences", Schema = "Ppl")]
  public partial class ContactReference
    {
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceID { get; set; }
        public string Reference { get; set; }
    }
}
