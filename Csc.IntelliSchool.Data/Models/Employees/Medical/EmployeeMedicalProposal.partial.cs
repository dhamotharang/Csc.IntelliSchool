
using Csc.Components.Common;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeMedicalProposal : NotifyObject {
    [IgnoreDataMember]
    [NotMapped]
    public int ItemCount {
      get {
        return Employees.Count() + Dependants.Count();
      }
    }



    public void AddRange(IEnumerable<Employee> items) {
      try {
        this.SuspendNotification();
        foreach (var itm in items)
          AddItem(itm);
        OnPropertyChanged(() => ItemCount, true);
      } finally {
        this.ResumeNotification();
      }
    }
    public void AddItem(Employee itm) {
      RemoveOldEmployee(itm.EmployeeID, itm.MedicalCertificate != null ? itm.MedicalCertificate.ProgramID : new int?());

      Employees.Add(new EmployeeMedicalProposalEmployee() {
        EmployeeID = itm.EmployeeID,
        Employee = itm,
        Concession = itm.MedicalCertificate != null ? itm.MedicalCertificate.Concession : new int?(),
        ProgramID = itm.MedicalCertificate != null ? itm.MedicalCertificate.ProgramID : new int?()
      });


      foreach (var dep in itm.Dependants) {
        Dependants.Add(new EmployeeMedicalProposalDependant() {
          DependantID = dep.DependantID,
          Dependant = dep,
          Concession = dep.MedicalCertificate != null ? dep.MedicalCertificate.Concession : new int?(),
          ProgramID = dep.MedicalCertificate != null ? dep.MedicalCertificate.ProgramID : new int?()
        });
      }

      OnPropertyChanged(() => ItemCount);
    }

    private void RemoveOldEmployee(int employeeId, int? programId) {
      var oldEmployees = Employees.Where(s => s.EmployeeID == employeeId && s.ProgramID == programId).ToArray();
      var oldDependants = Dependants.Where(s => s.Dependant != null && s.Dependant.EmployeeID == employeeId && s.ProgramID == programId).ToArray();

      foreach (var itm in oldEmployees)
        Employees.Remove(itm);

      foreach (var itm in oldDependants)
        Dependants.Remove(itm);
    }
  }
}