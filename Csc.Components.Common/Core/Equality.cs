using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Common {
  public class Equality {
    public static bool ValueEquality<T1, T2>(T1 val1, T2 val2)
      where T1 : IConvertible
      where T2 : IConvertible {
      // convert val2 to type of val1.
      T1 boxed2 = (T1)Convert.ChangeType(val2, typeof(T1));

      // compare now that same type.
      return val1.Equals(boxed2);
    }
  }
}