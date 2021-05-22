
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public class EmployeeLoanProxy : NotifyObject {
    private int _loanID;
    public int LoanID { get { return _loanID; } set { _loanID = value; OnPropertyChanged(() => LoanID); } }

    private int _employeeID;
    public int EmployeeID { get { return _employeeID; } set { _employeeID = value; OnPropertyChanged(() => EmployeeID); } }

    private string _notes;
    public string Notes { get { return _notes; } set { _notes = value; OnPropertyChanged(() => Notes); } }

    private DateTime? _requestDate;
    public DateTime? RequestDate { get { return _requestDate; } set { _requestDate = value; OnPropertyChanged(() => RequestDate); } }

    private int? _totalAmount;
    public int? TotalAmount { get { return _totalAmount; } set { _totalAmount = value; OnPropertyChanged(() => TotalAmount); } }

    private DateTime? _startMonth;

    public DateTime? StartMonth {
      get { return _startMonth; }
      set {
        _startMonth = value != null ? new DateTime(value.Value.Year, value.Value.Month, 1) : new DateTime?();
        OnPropertyChanged(() => StartMonth);
      }
    }

    private DateTime? _endMonth;

    public DateTime? EndMonth {
      get { return _endMonth; }
      set {
        _endMonth = value != null ? new DateTime(value.Value.Year, value.Value.Month, 1) : new DateTime?();
        OnPropertyChanged(() => EndMonth);
      }
    }

    private int? _installment;
    public int? Installment { get { return _installment; } set { _installment = value; OnPropertyChanged(() => Installment); } }

    private bool _newItem;
    public bool NewItem { get { return _newItem; } set { _newItem = value; OnPropertyChanged(() => NewItem); } }

    public static EmployeeLoanProxy CreateObject(DateTime? month) {
      return new EmployeeLoanProxy() { RequestDate = DateTime.Today, StartMonth = month.ToMonth(), EndMonth = month.ToMonth(), NewItem = true };
    }

    public static EmployeeLoanProxy CreateObject(EmployeeLoan item) {
      return new EmployeeLoanProxy() {
        EmployeeID = item.EmployeeID,
        LoanID = item.LoanID,
        RequestDate = item.RequestDate,
        TotalAmount = item.TotalAmount,
        StartMonth = item.StartMonth,
        EndMonth = item.EndMonth,
        Installment = item.Installment,
        Notes = item.Notes
      };
    }
  }

}