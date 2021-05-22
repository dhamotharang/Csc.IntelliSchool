using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeLoan : IChildrenObject {
    [IgnoreDataMember]
    [NotMapped]
    public object[] ChildObjects { get { return Installments != null ? Installments.ToArray() : new object[] { }; } }

    [IgnoreDataMember]
    [NotMapped]
    public int? Installment {
      get {
        if (Installments == null)
          return null;
        
        if (Installments.Count == 0)
          return 0;
        
        // TODO: check rounding
        return TotalAmount / Installments.Count();
      }
    }

    public static EmployeeLoan CreateObject(EmployeeLoanProxy proxy) {
      EmployeeLoan loan = new EmployeeLoan();
      loan.LoanID = proxy.LoanID;
      loan.RequestDate = proxy.RequestDate.Value;
      loan.EmployeeID = proxy.EmployeeID;
      loan.StartMonth = proxy.StartMonth.Value;
      loan.EndMonth = proxy.EndMonth.Value;
      loan.TotalAmount = proxy.TotalAmount.Value;
      loan.Notes = proxy.Notes;

      loan.Installments = EmployeeLoanInstallment.Generate(proxy);
      return loan;
    }

    public static explicit operator EmployeeLoan(EmployeeLoanProxy proxy) {
      return CreateObject(proxy);
    }
  }
}