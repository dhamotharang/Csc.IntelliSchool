using NodaTime;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Csc.Components.Common {

  public enum PeriodFilter {
    None,
    Current,
    Upcoming,
    Past,
    PreviousYear,
    PreviousMonth,
    All
  }

}
