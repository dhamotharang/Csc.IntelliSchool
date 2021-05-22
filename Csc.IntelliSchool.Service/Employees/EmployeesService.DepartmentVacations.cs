using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {

    #region Department Vacations
    public EmployeeDepartmentVacation AddOrUpdateEmployeeDepartmentVacation(EmployeeDepartmentVacation userItem) {
      using (var ent = CreateModel()) {
        var dbItem = ent.EmployeeDepartmentVacations.Query( EmployeeDepartmentVacationIncludes.Department)
          .SingleOrDefault(s => s.VacationID == userItem.VacationID);

        if (dbItem != null) {
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);

          ent.UpdateChildEntities(dbItem.Departments.ToArray(), userItem.Departments.ToArray(), (a, b) => a.DepartmentID == b.DepartmentID && a.VacationID == b.VacationID);
        } else
          ent.EmployeeDepartmentVacations.Add(userItem);

        ent.SaveChanges();

        ent.Logger.LogDatabase(CurrentUser, dbItem == null ? SystemLogDataAction.Insert : SystemLogDataAction.Update, typeof(EmployeeDepartmentVacation), userItem.VacationID.PackArray());
        ent.Logger.Flush();

        return ent.EmployeeDepartmentVacations.Query( EmployeeDepartmentVacationIncludes.None).Single(s => s.VacationID == userItem.VacationID);
      }
    }



    public EmployeeDepartmentVacation[] GetEmployeeDepartmentVacations(int departmentId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeDepartmentVacations
          .Query(new EmployeeDepartmentVacationDataCriteria() { DepartmentIDs = departmentId.PackArray() }
          .SetMonth(month, filter).As<EmployeeDepartmentVacationDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeDepartmentVacation[] GetEmployeeDepartmentVacationsByYear(int year) {
      DateTime yearDate = new DateTime(year, 1, 1);

      using (var ent = CreateModel()) {
        return ent.EmployeeDepartmentVacations.Query(EmployeeDepartmentVacationIncludes.Department,
          new EmployeeDepartmentVacationDataCriteria() {
            StartDate = yearDate,
            EndDate = yearDate.ToYearEnd()
          }).ToArray();
      }
    }

    public void DeleteDepartmentVacation(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeDepartmentVacation>(id);
      }
    }
    #endregion
  }

}

