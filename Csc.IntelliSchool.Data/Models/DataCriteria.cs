using Csc.Components.Common;
using System;

namespace Csc.IntelliSchool.Data {
  public abstract class DataCriteria {
    public T As<T>() where T : DataCriteria {
      return (T)this;
    }
  }

  public class RangeDataCriteria : DataCriteria, IDateRange {
    public DateTime? EndDate { get; set; }
    public DateTime? StartDate { get; set; }
    
    public RangeDataCriteria SetMonth(DateTime? month) {
      StartDate = month.ToMonth();
      EndDate = month.ToMonthEnd();

      return this;
    }

    public RangeDataCriteria SetMonth(DateTime? month, PeriodFilter filter) {
      if (month != null && filter != PeriodFilter.None) {
        var range = DateTimeExtensions.GetMonthOverlapFilterRange(month.Value, filter);

        this.StartDate = range.StartDate;
        this.EndDate = range.EndDate;
      } else
        StartDate = EndDate = null;

      return this;
    }

    public RangeDataCriteria SetRange(DateTime? startDate, DateTime? endDate) {
      this.StartDate = startDate;
      this.EndDate = endDate;

      return this;
    }

    public RangeDataCriteria SetRange(DateTime month, PeriodFilter filter) {
      DateTime startDate = DateTime.MinValue;
      DateTime endDate = DateTime.MaxValue;
      DateTimeExtensions.GetMonthOverlapFilterRange(month, filter, out startDate, out endDate);

      this.StartDate = startDate;
      this.EndDate = endDate;

      return this;
    }
  }

  public class ItemRangeDataCriteria  :RangeDataCriteria{
    public int[] ItemIDs { get; set; }

  }
}