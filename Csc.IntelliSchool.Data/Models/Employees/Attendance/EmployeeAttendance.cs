
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;


namespace Csc.IntelliSchool.Data {

  [Table("EmployeeAttendance", Schema = "HR")]
  public partial class EmployeeAttendance {

    public EmployeeAttendance() {
      this.TimeOffs = new HashSet<EmployeeAttendanceTimeOff>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int AttendanceID { get; set; }

    public System.DateTime Date { get; set; }
    public string Status { get; set; }

    public Nullable<System.TimeSpan> InTime { get; set; }
    public Nullable<System.TimeSpan> OutTime { get; set; }

    public Nullable<decimal> InPoints { get; set; }
    public Nullable<decimal> OutPoints { get; set; }

    public Nullable<decimal> InOvertimePoints { get; set; }
    public Nullable<decimal> OutOvertimePoints { get; set; }


    public Nullable<int> AbsencePoints { get; set; }
    public Nullable<int> ExtraAbsencePoints { get; set; }

    public bool IsEdited { get; set; }
    public string Notes { get; set; }
    public bool IsLocked { get; set; }

    [ForeignKey("Employee")]
    public int EmployeeID { get; set; }
    public virtual Employee Employee { get; set; }

    public virtual ICollection<EmployeeAttendanceTimeOff> TimeOffs { get; set; }
  }
}
