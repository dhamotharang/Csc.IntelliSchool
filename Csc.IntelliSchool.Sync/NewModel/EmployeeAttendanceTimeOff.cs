//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Csc.IntelliSchool.Sync.NewModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmployeeAttendanceTimeOff
    {
        public int TimeOffID { get; set; }
        public int AttendanceID { get; set; }
        public Nullable<System.TimeSpan> OutTime { get; set; }
        public Nullable<System.TimeSpan> InTime { get; set; }
        public Nullable<decimal> Points { get; set; }
        public bool IsEdited { get; set; }
        public bool IsManual { get; set; }
    
        public virtual EmployeeAttendance EmployeeAttendance { get; set; }
    }
}