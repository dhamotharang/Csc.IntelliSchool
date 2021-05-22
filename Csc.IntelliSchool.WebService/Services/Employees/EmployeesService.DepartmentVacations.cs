using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {

    #region Vacations
    public EmployeeDepartmentVacation AddOrUpdateEmployeeDepartmentVacation(EmployeeDepartmentVacation userItem) {
      using (var ent = ServiceManager.CreateModel()) {
        var dbItem = ent.EmployeeDepartmentVacations.Include("Departments").SingleOrDefault(s => s.VacationID == userItem.VacationID);

        if (dbItem != null) {
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);

          ent.UpdateChildEntities(dbItem.Departments.ToArray(), userItem.Departments.ToArray(), (a, b) => a.DepartmentID == b.DepartmentID && a.VacationID == b.VacationID);
        } else
          ent.EmployeeDepartmentVacations.Add(userItem);

        ent.SaveChanges();

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem == null ? SystemLogDataAction.Insert : SystemLogDataAction.Update, typeof(EmployeeDepartmentVacation), userItem.VacationID.PackArray());
        ent.Logger.Flush();

        return ent.EmployeeDepartmentVacations.Query().Single(s => s.VacationID == userItem.VacationID);
      }
    }


    public EmployeeVacation[] GetEmployeeVacations(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeVacations.Query(employeeId, month, filter).ToArray();
      }
    }

    public EmployeeDepartmentVacation[] GetEmployeeDepartmentVacations(int departmentId, DateTime month, PeriodFilter filter) {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeDepartmentVacations.Query(departmentId, month, filter).ToArray();
      }
    }

    public EmployeeDepartmentVacation[] GetEmployeeDepartmentVacationsByYear(int year) {
      DateTime yearDate = new DateTime(year, 1, 1);

      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeDepartmentVacations.Include("Departments.Department").Query(null, yearDate, yearDate.ToYearEnd()).ToArray();
      }
    }



    public void DeleteDepartmentVacation(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeDepartmentVacation>(id);
      }
    }
    #endregion
  }

}

