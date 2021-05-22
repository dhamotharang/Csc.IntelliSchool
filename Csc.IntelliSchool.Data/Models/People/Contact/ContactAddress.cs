
using System.ComponentModel.DataAnnotations; using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {
 [Table("ContactAddresses", Schema = "Ppl")]
  public partial class ContactAddress {
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AddressID { get; set; }
    public string Reference { get; set; }
    public bool IsDefault { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string ArabicAddress { get; set; }
    public string ArabicCity { get; set; }
    public string ArabicState { get; set; }
    public string ArabicCountry { get; set; }
    public Nullable<decimal> LocationLatitude { get; set; }
    public Nullable<decimal> LocationLongitude { get; set; }

    [ForeignKey("Contact")]
    public int ContactID { get; set; }
    public virtual Contact Contact { get; set; }
  }
}
