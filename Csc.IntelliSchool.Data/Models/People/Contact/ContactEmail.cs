using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Csc.IntelliSchool.Data {
  [Table("ContactEmails", Schema = "Ppl")]
  public partial class ContactEmail {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmailID { get; set; }
    public string Reference { get; set; }
    public string Email { get; set; }
    public bool IsDefault { get; set; }

    [ForeignKey("Contact")]
    public int ContactID { get; set; }
    public virtual Contact Contact { get; set; }
  }
}
