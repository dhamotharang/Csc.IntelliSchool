using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public abstract class SystemLogDataEntryBase {
    public virtual SystemLogData[] GetData() {
      var properties = this.GetType().GetProperties();
      if (properties.Count() == 0)
        return new SystemLogData[] { };

      List<SystemLogData> lst = new List<SystemLogData>(properties.Count());
      foreach (var prop in properties) {
        var propData = prop.GetValue(this);
        if (propData == null)
          continue;

        SystemLogData data = new SystemLogData();
        data.Property = prop.Name;

        int[] enumData = propData as int[];
        if (enumData != null && prop.IsCollection()) {
          var propVals = enumData.Cast<object>().ToArray();
          data.Value = string.Format("{0}#{1}", propVals.Count(), string.Join("|", propVals.Select(s => s.ToString())));
        } else
          data.Value = propData.ToString();

        lst.Add(data);
      }

      return lst.ToArray();
    }
  }


}