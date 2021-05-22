using System;
using System.Linq;
using Csc.Components.Common;

namespace Csc.IntelliSchool.Service {
  public partial class PeopleService {
    public string[] GetContactReferences() {
      using (var ent = CreateModel()) {
        return
          ent.ContactReferences.Select(s => s.Reference).ToArray()
          .Concat(ent.ContactNumbers.Where(s => s.Reference != null).Select(s => s.Reference).ToArray())
          .Concat(ent.ContactAddresses.Where(s => s.Reference != null).Select(s => s.Reference).ToArray())
          .Select(s => s.Trim().ToTitleCase()).Where(s => s.Length > 0).Distinct(StringComparer.CurrentCultureIgnoreCase).ToArray();
      }
    }
  }
}
