
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public interface IEmployeeShift {
    TimeSpan? FromMargin { get; }
    TimeSpan? ToMargin { get; }


    TimeSpan? SaturdaysFrom { get; }
    TimeSpan? SaturdaysTo { get; }

    TimeSpan? SundaysFrom { get; }
    TimeSpan? SundaysTo { get; }

    TimeSpan? MondaysFrom { get; }
    TimeSpan? MondaysTo { get; }

    TimeSpan? TuesdaysFrom { get; }
    TimeSpan? TuesdaysTo { get; }

    TimeSpan? WednesdaysFrom { get; }
    TimeSpan? WednesdaysTo { get; }

    TimeSpan? ThursdaysFrom { get; }
    TimeSpan? ThursdaysTo { get; }

    TimeSpan? FridaysFrom { get; }
    TimeSpan? FridaysTo { get; }


    DayOfWeek[] GetWeekends();
    TimeSpan? GetStart(DayOfWeek day);
    TimeSpan? GetEnd(DayOfWeek day);
  }


}