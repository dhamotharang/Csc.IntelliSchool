using System;
using System.Collections.Generic;
using System.Linq;


namespace Csc.Components.Data {
  public static class DataExtensions {
    public static string[] GetIncludes(Enum value, string relation = null) {
      var type = value.GetType();
      var fields = Enum.GetValues(type).Cast<Enum>()
        .Where(s => value.HasFlag(s))
        .Select(s => type.GetField(s.ToString()));

      List<string> items = new List<string>();
      foreach (var fld in fields) {
        var attList = fld.GetCustomAttributes(typeof(DataIncludeAttribute), false).Cast<DataIncludeAttribute>();

        if (attList == null || attList.Count() == 0) {
          //if ((int)Enum.Parse(type, fld.Name) > -0)
          //  items.Add(fld.Name);
          continue;
        }

        foreach (var att in attList) {
          if (att.IncludePaths != null && att.IncludePaths.Length  > 0)
            items.AddRange(att.IncludePaths);
          else
            items.Add(fld.Name);
        }
      }

      var tmpItems = items.Distinct().Where(s => s.Length > 0);

      return relation == null ? tmpItems.ToArray() : tmpItems.Select(s=> relation + "." + s).ToArray();
    }
  }
}
