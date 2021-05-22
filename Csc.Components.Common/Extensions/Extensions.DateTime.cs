using System;
using System.Linq;
using System.Collections.Generic;
using NodaTime;

namespace Csc.Components.Common {
  public static class DateTimeExtensions {
    #region TimeSpan
    public static TimeSpan? Sum(this IEnumerable<TimeSpan?> source) {
      var items = source.Where(s => s != null).ToArray();
      if (items.Count() == 0)
        return new TimeSpan?();

      return new TimeSpan(items.Sum(s => s.Value.Ticks));

    }

    public static TimeSpan? ToMinutes(this TimeSpan? value) {
      if (value == null)
        return null;

      return ToMinutes(value.Value);
    }
    public static TimeSpan ToMinutes(this TimeSpan value) {
      return new TimeSpan(value.Hours, value.Minutes, 0);
    }

    #endregion



    #region Periods
    /// <summary>
    /// Calculate period until today.
    /// </summary>
    public static TimePeriod CalculatePeriod(DateTime startDate) {
      return CalculatePeriod(startDate, DateTime.Today);
    }
    public static TimePeriod CalculatePeriod(DateTime startDate, DateTime endDate) {
      return new Common.TimePeriod(Period.Between(startDate.ToLocalDate(), endDate.ToLocalDate(), PeriodUnits.AllDateUnits));
    }
    #endregion

    #region ToDateTime
    public static DateTime ToDateTime(this TimeSpan time) {
      return (DateTime)ToDateTime((TimeSpan?)time);
    }
    public static DateTime? ToDateTime(this TimeSpan? time) {
      if (time == null)
        return null;
      return DateTime.Today.Add(time.Value);
    }
    #endregion

    #region ToYear
    public static DateTime ToYear(this DateTime date) {
      return (DateTime)ToYear((DateTime?)date);
    }
    public static DateTime? ToYear(this DateTime? date) {
      if (date == null)
        return null;
      return new DateTime(date.Value.Year, 1, 1);
    }

    public static DateTime ToYearEnd(this DateTime date) {
      return (DateTime)ToYearEnd((DateTime?)date);
    }
    public static DateTime? ToYearEnd(this DateTime? date) {
      if (date == null)
        return null;
      return new DateTime(date.Value.Year, 1, 1).AddYears(1).AddDays(-1);
    }
    #endregion

    #region ToMonth
    public static IEnumerable<DateTime> GetMonthDays(this DateTime month) {
      month = month.ToMonth();
      DateTime end = month.ToMonthEnd();
      while (month <= end) {
        yield return month;

        month = month.AddDays(1);
      }
    }
    public static DateTime ToMonth(this DateTime date) {
      return (DateTime)ToMonth((DateTime?)date);
    }
    public static DateTime? ToMonth(this DateTime? date) {
      if (date == null)
        return null;
      return new DateTime(date.Value.Year, date.Value.Month, 1);
    }

    public static DateTime ToMonthEnd(this DateTime date) {
      return (DateTime)ToMonthEnd((DateTime?)date);
    }
    public static DateTime? ToMonthEnd(this DateTime? date) {
      if (date == null)
        return null;
      return new DateTime(date.Value.Year, date.Value.Month, 1).AddMonths(1).AddDays(-1);
    }
    #endregion

    #region ToDay
    public static DateTime ToDay(this DateTime date) {
      return (DateTime)ToDay((DateTime?)date);
    }
    public static DateTime? ToDay(this DateTime? date) {
      if (date == null)
        return null;
      return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day);
    }

    public static DateTime ToDayEnd(this DateTime date) {
      return (DateTime)ToDayEnd((DateTime?)date);
    }
    public static DateTime? ToDayEnd(this DateTime? date) {
      if (date == null)
        return null;
      return date.ToDay().Value.AddDays(1).AddSeconds(-1);
    }
    #endregion

    public static LocalDate ToLocalDate(this DateTime dateTime) {
      return new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }

    public static void GetMonthOverlapFilterRange(DateTime month, PeriodFilter filter, out DateTime startDate, out DateTime endDate) {
      month = new DateTime(month.Year, month.Month, 1);
      startDate = month;
      endDate = month.AddMonths(1).AddDays(-1);

      if (filter == PeriodFilter.Upcoming) {
        startDate = month.AddMonths(1);
        endDate = DateTime.MaxValue;
      } else if (filter == PeriodFilter.Past) {
        startDate = DateTime.MinValue;
        endDate = month.AddDays(-1);
      }
    }
    public static DateRange GetMonthOverlapFilterRange(DateTime month, PeriodFilter filter) {
      DateTime startDate, endDate;

      GetMonthOverlapFilterRange(month, filter, out startDate, out endDate);

      return new Common.DateRange(startDate, endDate);
    }

    public static bool Overlaps(DateTime month, PeriodFilter filter, DateTime startDate2, DateTime endDate2) {
      DateTime startDate1, endDate1;

      GetMonthOverlapFilterRange(month, filter, out startDate1, out endDate1);

      return Overlaps(startDate1, endDate1, startDate2, endDate2);
    }

    public static bool Overlaps(this DateTime date, DateTime startDate2, DateTime endDate2) {
      return date >= startDate2 && date <= startDate2;
    }

    public static bool Overlaps(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2) {
      return startDate1 <= endDate2 && startDate2 <= endDate1;
    }
  }
}


//public static int? CalculatePeriodYears(DateTime? date) { return date == null ? new int?() : CalculatePeriodYears(date.Value); }
//public static int CalculatePeriodYears(DateTime date) { return CalculatePeriodYears(date, DateTime.Today); }
//public static int CalculatePeriodYears(DateTime date1, DateTime date2) {
//  int years, months;

//  CalculatePeriod(date1, date2, out years, out months);
//  return years;
//}


//public static void CalculatePeriod(DateTime date, out int years, out int months) { CalculatePeriod(date, DateTime.Today, out years, out months); }
//public static void CalculatePeriod(DateTime date1, DateTime date2, out int years, out int months) {
//  //assumes date2 is the bigger date for simplicity

//  //years
//  TimeSpan diff = date2 - date1;
//  years = diff.Days / 366;
//  DateTime workingDate = date1.AddYears(years);

//  while (workingDate.AddYears(1) <= date2) {
//    workingDate = workingDate.AddYears(1);
//    years++;
//  }

//  //months
//  diff = date2 - workingDate;
//  months = diff.Days / 31;
//  workingDate = workingDate.AddMonths(months);

//  while (workingDate.AddMonths(1) <= date2) {
//    workingDate = workingDate.AddMonths(1);
//    months++;
//  }

//  //weeks and days
//  diff = date2 - workingDate;
//}
