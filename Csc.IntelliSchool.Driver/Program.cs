using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Driver {
  class Program {
    static void Main(string[] args) {
      Entities ent = new Driver.Entities();

      IntList lst =new Driver.IntList();
      lst.Values = new int[] { 3 };
      foreach (var r in lst.Values) {
        Console.WriteLine(r);
      }


      ent.EmployeeDeleteDuplicateAttendance(new DateTime(2017, 1, 1), new DateTime(2017, 1, 2), lst);



    }
  }

  public class IntList : DataTable {
    public IntList() {
      this.Columns.Add("Value", typeof(int));
    }

    public int[] Values {
      get {
        int[] vals = new int[this.Rows.Count];
        for (int i = 0; i < this.Rows.Count; i++) {
          vals[i] = this.Rows[i].Field<int>(0);
        }
        return vals;
      }
      set {
        this.Rows.Clear();
        foreach (var val in value)
          this.Rows.Add(new object[] { val });
      }
    }


  }
}
