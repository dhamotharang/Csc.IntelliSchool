
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

 [Table("Contacts", Schema = "Ppl")]
  public partial class Contact {

    public Contact() {
      this.Addresses = new HashSet<ContactAddress>();
      this.Numbers = new HashSet<ContactNumber>();
      this.ContactEmails = new HashSet<ContactEmail>();
    }

    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ContactID { get; set; }


    public virtual ICollection<ContactAddress> Addresses { get; set; }

    public virtual ICollection<ContactNumber> Numbers { get; set; }

    public virtual ICollection<ContactEmail> ContactEmails { get; set; }
  }
}
