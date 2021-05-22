using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Common {
  public class TimePeriod {
    public int Days { get; set; }
    public int Months { get; set; }
    public int Years { get; set; }


    public TimePeriod() {

    }
    internal TimePeriod(Period prd) {
      Days = prd.Days;
      Months = prd.Months;
      Years = prd.Years;
    }
  }
}
