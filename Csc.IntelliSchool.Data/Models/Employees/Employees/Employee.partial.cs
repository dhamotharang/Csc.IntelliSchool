
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {


  public partial class Employee : IChildrenObject {
    private EmployeeMedicalInfo _medicalInfo;
    [IgnoreDataMember]
    [NotMapped]
    public EmployeeMedicalInfo MedicalInfo {
      get {
        if (null == _medicalInfo)
          lock (this)
            if (null == _medicalInfo)
              _medicalInfo = new EmployeeMedicalInfo(this);
        return _medicalInfo;
      }
    }


    [IgnoreDataMember]
    [NotMapped]
    public object[] ChildObjects { get { return Dependants != null ? Dependants.ToArray() : new object[] { }; } }


    #region Employment

    [IgnoreDataMember]
    [NotMapped]
    public int HireYears {
      get {
        if (IsTerminated == false)
          return (int)DateTimeExtensions.CalculatePeriod(HireDate).Years;
        else
          return (int)DateTimeExtensions.CalculatePeriod(HireDate, TerminationDate.Value).Years;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public int HireMonths {
      get {
        if (IsTerminated == false)
          return (int)DateTimeExtensions.CalculatePeriod(HireDate).Months;
        else
          return (int)DateTimeExtensions.CalculatePeriod(HireDate, TerminationDate.Value).Months;
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public bool IsTerminated { get { return this.TerminationDate != null; } }
    [IgnoreDataMember]
    [NotMapped]
    public bool IsTerminalUser { get { return TerminalID != null && TerminalUserID != null && TerminalUserID > 0; } }

    [IgnoreDataMember]
    [NotMapped]
    public string FullPositionString {
      get {
        string str = string.Empty;

        if (Position != null)
          str += Position.Name;

        if (Department != null) {
          if (str.Length > 0)
            str += ", ";

          str += Department.Name;
        }

        if (Branch != null) {
          bool needsClose = false;
          if (str.Length > 0) {
            needsClose = true;
            str += " (";
          }

          str += Branch.Name;
          if (needsClose)
            str += ")";
        }

        return str.Length > 0 ? str.Trim() : null;
      }
    }
    [IgnoreDataMember]
    [NotMapped]
    public string FullTerminationString {
      get {
        if (this.TerminationDate == null)
          return null;

        string str = this.TerminationDate.Value.ToShortDateString();
        if (this.TerminationReason != null && this.TerminationReason.Trim().Length > 0)
          str += string.Format(", {0}", this.TerminationReason.Trim());

        return str;
      }
    }
    #endregion Employment

    public static Employee CreateObject() {
      Employee emp = new Employee();
      emp.Person = new Data.Person();
      emp.Person.Contact = new Data.Contact();
      emp.Salary = new EmployeeSalary();
      return emp;
    }

    public bool IsMonthEmployee(DateTime month) {
      DateTime startDate = month.ToMonth();
      DateTime endDate = month.ToMonthEnd();

      return HireDate <= endDate && (TerminationDate == null || (TerminationDate >= startDate && TerminationHide != true));
    }
  }

}