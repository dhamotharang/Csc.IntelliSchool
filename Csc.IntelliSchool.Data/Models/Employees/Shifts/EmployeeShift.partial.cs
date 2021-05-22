
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class EmployeeShift : IEmployeeShift, IChildrenObject {
    [IgnoreDataMember]
    [NotMapped]
    public object[] ChildObjects { get { return Overrides != null ? Overrides.ToArray() : new object[] { }; } }

    #region Common
    public IEmployeeShift SelectShift(DateTime date) {
      date = date.ToDay();
      IEmployeeShift shift = this.Overrides.Where(s => date >= s.StartDate && date <= s.EndDate && s.IsActive == true).OrderBy(s => s.Order).ThenBy(s => s.OverrideID).LastOrDefault();
      if (null == shift)
        shift = this;
      return shift;
    }
    public bool SelectIsWeekend(DateTime date) {
      return SelectShift(date).GetWeekends().Contains(date.DayOfWeek);
    }
    public TimeSpan? SelectStartTime(DateTime date) {
      return SelectShift(date).GetStart(date.DayOfWeek);
    }
    public TimeSpan? SelectEndTime(DateTime date) {
      return SelectShift(date).GetEnd(date.DayOfWeek);
    }
    public TimeSpan SelectFromMargin(DateTime date) {
      return SelectShift(date).FromMargin;
    }
    public TimeSpan SelectToMargin(DateTime date) {
      return SelectShift(date).ToMargin;
    }
    public bool  SelectCalculateIsDayOvertime(DateTime date) {
      return SelectShift(date).CalculateDayOvertime;
    }
    public bool SelectCalculateIsWeekendOvertime(DateTime date) {
      return SelectShift(date).CalculateWeekendOvertime;
    }
    public bool SelectCalculateIsVacationOvertime(DateTime date) {
      return SelectShift(date).CalculateVacationOvertime;
    }
    public bool SelectCalculateTimeOffs(DateTime date) {
      return SelectShift(date).CalculateTimeOffs;
    }
    #endregion

    public DayOfWeek[] GetWeekends() {
      List<DayOfWeek> list = new List<DayOfWeek>(7);
      if (Saturdays == false)
        list.Add(DayOfWeek.Saturday);
      if (Sundays == false)
        list.Add(DayOfWeek.Sunday);
      if (Mondays == false)
        list.Add(DayOfWeek.Monday);
      if (Tuesdays == false)
        list.Add(DayOfWeek.Tuesday);
      if (Wednesdays == false)
        list.Add(DayOfWeek.Wednesday);
      if (Thursdays == false)
        list.Add(DayOfWeek.Thursday);
      if (Fridays == false)
        list.Add(DayOfWeek.Friday);
      return list.ToArray();
    }
    public TimeSpan? GetStart(DayOfWeek day) {
      switch (day) {
        case DayOfWeek.Saturday:
          return Saturdays ? SaturdaysFrom : null;

        case DayOfWeek.Sunday:
          return Sundays ? SundaysFrom : null;

        case DayOfWeek.Monday:
          return Mondays ? MondaysFrom : null;

        case DayOfWeek.Tuesday:
          return Tuesdays ? TuesdaysFrom : null;

        case DayOfWeek.Wednesday:
          return Wednesdays ? WednesdaysFrom : null;

        case DayOfWeek.Thursday:
          return Thursdays ? ThursdaysFrom : null;

        case DayOfWeek.Friday:
          return Fridays ? FridaysFrom : null;

        default:
          return null;
      }
    }
    public TimeSpan? GetEnd(DayOfWeek day) {
      switch (day) {
        case DayOfWeek.Saturday:
          return Saturdays ? SaturdaysTo : null;

        case DayOfWeek.Sunday:
          return Sundays ? SundaysTo : null;

        case DayOfWeek.Monday:
          return Mondays ? MondaysTo : null;

        case DayOfWeek.Tuesday:
          return Tuesdays ? TuesdaysTo : null;

        case DayOfWeek.Wednesday:
          return Wednesdays ? WednesdaysTo : null;

        case DayOfWeek.Thursday:
          return Thursdays ? ThursdaysTo : null;

        case DayOfWeek.Friday:
          return Fridays ? FridaysTo : null;

        default:
          return null;
      }
    }

    public bool Validate() {
      Func<TimeSpan?, TimeSpan?, bool> validate = (from, to) => (from == null && to == null) || (from != null && to != null && to > from);

      if (validate(SaturdaysFrom, SaturdaysTo) == false)
        return false;
      if (validate(SundaysFrom, SundaysTo) == false)
        return false;
      if (validate(MondaysFrom, MondaysTo) == false)
        return false;
      if (validate(TuesdaysFrom, TuesdaysTo) == false)
        return false;
      if (validate(WednesdaysFrom, TuesdaysTo) == false)
        return false;
      if (validate(ThursdaysFrom, ThursdaysTo) == false)
        return false;
      if (validate(FridaysFrom, FridaysTo) == false)
        return false;

      return true;
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool Saturdays { get { return SaturdaysFrom != null && SaturdaysTo != null && SaturdaysFrom < SaturdaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Sundays { get { return SundaysFrom != null && SundaysTo != null && SundaysFrom < SundaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Mondays { get { return MondaysFrom != null && MondaysTo != null && MondaysFrom < MondaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Tuesdays { get { return TuesdaysFrom != null && TuesdaysTo != null && TuesdaysFrom < TuesdaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Wednesdays { get { return WednesdaysFrom != null && WednesdaysTo != null && WednesdaysFrom < WednesdaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Thursdays { get { return ThursdaysFrom != null && ThursdaysTo != null && ThursdaysFrom < ThursdaysTo; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool Fridays { get { return FridaysFrom != null && FridaysTo != null && FridaysFrom < FridaysTo; } }

    [IgnoreDataMember]
    [NotMapped]
    public string SaturdaysString {
      get {
        if (this.SaturdaysFrom == null || this.SaturdaysTo == null)
          return null;

        var diff = this.SaturdaysTo.Value - this.SaturdaysFrom.Value;
        var from = DateTime.Today.Add(this.SaturdaysFrom.Value);
        var to = DateTime.Today.Add(this.SaturdaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string SundaysString {
      get {
        if (this.SundaysFrom == null || this.SundaysTo == null)
          return null;

        var diff = this.SundaysTo.Value - this.SundaysFrom.Value;
        var from = DateTime.Today.Add(this.SundaysFrom.Value);
        var to = DateTime.Today.Add(this.SundaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string MondaysString {
      get {
        if (this.MondaysFrom == null || this.MondaysTo == null)
          return null;

        var diff = this.MondaysTo.Value - this.MondaysFrom.Value;
        var from = DateTime.Today.Add(this.MondaysFrom.Value);
        var to = DateTime.Today.Add(this.MondaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string TuesdaysString {
      get {
        if (this.TuesdaysFrom == null || this.TuesdaysTo == null)
          return null;

        var diff = this.TuesdaysTo.Value - this.TuesdaysFrom.Value;
        var from = DateTime.Today.Add(this.TuesdaysFrom.Value);
        var to = DateTime.Today.Add(this.TuesdaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string WednesdaysString {
      get {
        if (this.WednesdaysFrom == null || this.WednesdaysTo == null)
          return null;

        var diff = this.WednesdaysTo.Value - this.WednesdaysFrom.Value;
        var from = DateTime.Today.Add(this.WednesdaysFrom.Value);
        var to = DateTime.Today.Add(this.WednesdaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string ThursdaysString {
      get {
        if (this.ThursdaysFrom == null || this.ThursdaysTo == null)
          return null;

        var diff = this.ThursdaysTo.Value - this.ThursdaysFrom.Value;
        var from = DateTime.Today.Add(this.ThursdaysFrom.Value);
        var to = DateTime.Today.Add(this.ThursdaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public string FridaysString {
      get {
        if (this.FridaysFrom == null || this.FridaysTo == null)
          return null;

        var diff = this.FridaysTo.Value - this.FridaysFrom.Value;
        var from = DateTime.Today.Add(this.FridaysFrom.Value);
        var to = DateTime.Today.Add(this.FridaysTo.Value);

        return string.Format("{0:hh:mm tt} - {1:hh:mm tt} ({2:hh\\:mm})", from, to, diff);
      }
    }
  }

}