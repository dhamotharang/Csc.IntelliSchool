
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {

  [Table("EmployeeAttendanceTimeOffs", Schema = "HR")]
  public partial class EmployeeAttendanceTimeOff {
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TimeOffID { get; set; }
    public Nullable<System.TimeSpan> OutTime { get; set; }
    public Nullable<System.TimeSpan> InTime { get; set; }
    public Nullable<decimal> Points { get; set; }
    public bool IsEdited { get; set; }
    public bool IsManual { get; set; }

    [ForeignKey("EmployeeAttendance")]
    public int AttendanceID { get; set; }
    public virtual EmployeeAttendance EmployeeAttendance { get; set; }
  }
}
