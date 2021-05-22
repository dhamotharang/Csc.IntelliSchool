using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Data {
  [Flags]
  public enum TableChangeType {
    None = 0,
    Insert = 1 << 0,
    Update = 1 << 1,
    Delete = 1 << 2,
    All = Insert | Update | Delete
  }
}
