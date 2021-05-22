
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Csc.IntelliSchool.Data {

  [Table("ContactNumbers", Schema = "Ppl")]
  public partial class ContactNumber {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int NumberID { get; set; }
    public string Number { get; set; }
    public string Reference { get; set; }
    public bool IsDefault { get; set; }

    [ForeignKey("Contact")]
    public int ContactID { get; set; }
    public virtual Contact Contact { get; set; }
  }
}
