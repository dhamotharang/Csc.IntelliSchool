using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;

namespace Csc.Components.Common.Data {
  [Flags]
  public enum DynObjectColumnFlags {
    None = 0,
    Header = 1 << 0,
    Hidden = 1 << 1,

    Boolean = 1 << 2,
    TrueFalse = 1 << 3,
    Date = 1 << 4,
    Text = 1 << 5,
    Number = 1 << 6
  }

}
