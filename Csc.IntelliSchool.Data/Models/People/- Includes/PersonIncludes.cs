using Csc.Components.Data;
using System;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum PersonIncludes {
    None = 0,
    [DataInclude("Nationality")]
    Nationality = 1 << 1,
    [DataInclude("Religion")]
    Religion = 1 << 2,

    [DataInclude("Contact.Addresses", "Contact.Numbers")]
    Contact = 1 << 3,


    All = Nationality | Religion | Contact
  }

}