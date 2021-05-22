using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;
using System.ServiceModel.Web;
using System.Diagnostics;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {

    public EmployeeContactDetails[] GetEmployeeContactDetails(DateTime month, int[] listIds, int[] employeeIds) {
      var userId = WebOperationContext.Current.IncomingRequest.Headers["UserID"];
      Trace.WriteLine(userId);


      List<EmployeeContactDetails> result = new List<EmployeeContactDetails>();
      month = month.ToMonth();

      using (var ent = ServiceManager.CreateModel()) {
        var employees = ent.Employees.Query(EmployeeDataFilter.Employment | EmployeeDataFilter.Contact, month, listIds, employeeIds).ToArray();

        foreach (var emp in employees) {
          EmployeeContactDetails contactDetails = new EmployeeContactDetails(emp);
          if (emp.Contact == null)
            continue;

          InternalFillEmployeeContactNumbers(emp, contactDetails);

          InternalFillEmployeeContactAddresses(emp, contactDetails);

          result.Add(contactDetails);
        }
      }

      return result.OrderBy(s => s.FullName).ToArray();
    }

    private static void InternalFillEmployeeContactAddresses(Employee emp, EmployeeContactDetails contactDetails) {
      int i = 1;
      foreach (var num in emp.Contact.Addresses.OrderBy(s => s.IsDefault).ThenBy(s => s.AddressID)) {
        var numStr = string.Empty;

        if (string.IsNullOrWhiteSpace(num.Address))
          continue;

        if (string.IsNullOrWhiteSpace(num.Reference) == false) {
          numStr += string.Format("({0}) ", num.Reference.Trim());
        }

        numStr += num.Address.Trim();

        contactDetails.SetContactAddress(i.ToString(), numStr);

        i++;
      }
    }

    private static void InternalFillEmployeeContactNumbers(Employee emp, EmployeeContactDetails contactDetails) {
      int i = 1;
      foreach (var num in emp.Contact.Numbers.OrderBy(s => s.IsDefault).ThenBy(s => s.NumberID)) {
        var numStr = string.Empty;

        if (string.IsNullOrWhiteSpace(num.Number))
          continue;

        contactDetails.SetContactNumber(i, num.Number, (num.Reference ?? string.Empty).Trim());

        i++;
      }
    }

  }

}
