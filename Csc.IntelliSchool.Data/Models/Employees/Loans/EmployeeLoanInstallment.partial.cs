
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  public partial class EmployeeLoanInstallment {

    public static EmployeeLoanInstallment[] Generate(EmployeeLoanProxy loan) {
      DateTime startMonth = loan.StartMonth.Value;
      int total = 0;
      List<EmployeeLoanInstallment> installments = new List<EmployeeLoanInstallment>();
      while (startMonth <= loan.EndMonth) {
        EmployeeLoanInstallment inst = new EmployeeLoanInstallment();
        inst.LoanID = loan.LoanID;
        inst.Month = startMonth;
        if (total + loan.Installment.Value < loan.TotalAmount.Value)
          inst.Amount = loan.Installment.Value;
        else
          inst.Amount = loan.TotalAmount.Value - total;

        total += inst.Amount;
        installments.Add(inst);

        startMonth = startMonth.AddMonths(1);
      }

      return installments.ToArray();
    }
  }

}