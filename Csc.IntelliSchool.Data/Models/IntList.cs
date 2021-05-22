using Csc.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace Csc.IntelliSchool.Data {
  public class IntList : DataTable {
    public IntList() {
      this.Columns.Add("Value", typeof(int));
    }

    public IntList(IEnumerable<int> values) : this() {
      this.Values = values;
    }

    public IEnumerable<int> Values {
      get {
        int[] vals = new int[this.Rows.Count];
        for (int i = 0; i < this.Rows.Count; i++) {
          vals[i] = (int)this.Rows[i][0];
        }
        return vals;
      }
      set {
        this.Rows.Clear();
        foreach (var val in value)
          this.Rows.Add(new object[] { val });
      }
    }

    public static explicit operator IntList(int[] values) {
      return new Data.IntList(values);
    }
  }
}