
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeMedicalRequest : NotifyObject {
    [IgnoreDataMember]
    [NotMapped]
    public int ItemCount { get { return Employees.Count() + Dependants.Count(); } }

    public IEnumerable<IEmployeeMedicalRequestItem> GetRequestItems() {
      return Employees.Select(s => (IEmployeeMedicalRequestItem)s).Concat(Dependants.Select(s => (IEmployeeMedicalRequestItem)s)).ToArray();
    }


    public void SetRequestItems(IEnumerable<IEmployeeMedicalRequestItem> items) {
      var filterItems = items.Where(s => s.Owner == EmployeeMedicalCertificateOwner.Employee);
      var currentIds = Employees.Select(s => s.EmployeeID).ToArray();
      var targetIds = filterItems.Select(s => s.OwnerID).ToArray();

      var toDeleteIds = currentIds.Except(targetIds).ToArray();
      foreach (var id in toDeleteIds)
        Employees.Remove(Employees.Single(s => s.EmployeeID == id));

      var toUpdateIds = targetIds.Intersect(currentIds).ToArray();
      foreach (var id in toUpdateIds)
        Employees.Single(s => s.EmployeeID == id).CertificateCode = filterItems.Single(s => s.OwnerID == id).CertificateCode;



      filterItems = items.Where(s => s.Owner == EmployeeMedicalCertificateOwner.Dependant);
      currentIds = Dependants.Select(s => s.DependantID).ToArray();
      targetIds = filterItems.Select(s => s.OwnerID).ToArray();

      toDeleteIds = currentIds.Except(targetIds).ToArray();
      foreach (var id in toDeleteIds)
        Dependants.Remove(Dependants.Single(s => s.DependantID == id));

      toUpdateIds = targetIds.Intersect(currentIds).ToArray();
      foreach (var id in toUpdateIds)
        Dependants.Single(s => s.DependantID == id).CertificateCode = filterItems.Single(s => s.OwnerID == id).CertificateCode;

      OnPropertyChanged(() => ItemCount);
    }


    public void AddItem(object itm, string certCode) {
      if (itm is Employee) {
        var emp = (Employee)itm;

        var oldItem = Employees.SingleOrDefault(s => s.EmployeeID == emp.EmployeeID);
        if (oldItem != null)
          Employees.Remove(oldItem);

        Employees.Add(new EmployeeMedicalRequestEmployee() {
          CertificateCode = certCode,
          Employee = emp,
          EmployeeID = emp.EmployeeID,
        });
      } else if (itm is EmployeeDependant) {
        var dep = (EmployeeDependant)itm;
        var oldItem = Dependants.SingleOrDefault(s => s.DependantID == dep.DependantID);
        if (oldItem != null)
          Dependants.Remove(oldItem);

        Dependants.Add(new EmployeeMedicalRequestDependant() {
          CertificateCode = certCode,
          Dependant = dep,
          DependantID = dep.DependantID,
        });
      } else
        throw new ArgumentException();

      OnPropertyChanged(() => ItemCount);
    }
  }


}