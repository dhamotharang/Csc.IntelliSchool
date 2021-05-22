using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Common {
  public class DateRange : IDateRange {
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsValid { get { return false == (StartDate != null && EndDate != null && EndDate < StartDate); } }

    public DateRange() { }
    public DateRange(DateTime? startDate, DateTime? endDate) {
      this.StartDate = startDate;
      this.EndDate = endDate;
    }
  }

  public interface IDateRange {
    DateTime? StartDate { get; set; }
    DateTime? EndDate { get; set; }
  }

}
