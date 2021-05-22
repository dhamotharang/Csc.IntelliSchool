using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Data {
  public partial class Contact {
    [IgnoreDataMember][NotMapped]
    public ContactNumber DefaultNumber
    {
      get
      {
        if (Numbers == null)
          return null;
        var def = Numbers.Where(s => s.IsDefault == true).FirstOrDefault();
        if (def == null)
          def = Numbers.FirstOrDefault();

        return def;
      }
    }

    [IgnoreDataMember][NotMapped]
    public ContactAddress DefaultAddress
    {
      get
      {
        if (Addresses == null)
          return null;
        var def = Addresses.Where(s => s.IsDefault == true).FirstOrDefault();
        if (def == null)
          def = Addresses.FirstOrDefault();

        return def;
      }
    }
  }
}