
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeShiftOverrides", Schema = "HR")]
  public partial class EmployeeShiftOverride {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int OverrideID { get; set; }

    public Nullable<int> Order { get; set; }
    public bool IsActive { get; set; }

    public bool CalculateDayOvertime { get; set; }
    public bool CalculateWeekendOvertime { get; set; }
    public bool CalculateVacationOvertime { get; set; }

    public bool CalculateTimeOffs { get; set; }

    public System.DateTime StartDate { get; set; }
    public System.DateTime EndDate { get; set; }


    public Nullable<System.TimeSpan> SaturdaysFrom { get; set; }
    public Nullable<System.TimeSpan> SaturdaysTo { get; set; }
    public Nullable<System.TimeSpan> SundaysFrom { get; set; }
    public Nullable<System.TimeSpan> SundaysTo { get; set; }
    public Nullable<System.TimeSpan> MondaysFrom { get; set; }
    public Nullable<System.TimeSpan> MondaysTo { get; set; }
    public Nullable<System.TimeSpan> TuesdaysFrom { get; set; }
    public Nullable<System.TimeSpan> TuesdaysTo { get; set; }
    public Nullable<System.TimeSpan> WednesdaysFrom { get; set; }
    public Nullable<System.TimeSpan> WednesdaysTo { get; set; }
    public Nullable<System.TimeSpan> ThursdaysFrom { get; set; }
    public Nullable<System.TimeSpan> ThursdaysTo { get; set; }
    public Nullable<System.TimeSpan> FridaysFrom { get; set; }
    public Nullable<System.TimeSpan> FridaysTo { get; set; }

    public System.TimeSpan FromMargin { get; set; }
    public System.TimeSpan ToMargin { get; set; }

    public string Notes { get; set; }

    [ForeignKey("Shift")]
    public int ShiftID { get; set; }
    public virtual EmployeeShift Shift { get; set; }

    [ForeignKey("Type")]
    public int? TypeID { get; set; }
    public virtual EmployeeShiftOverrideType Type { get; set; }
  }
}
