using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Vacations
    public EmployeeVacation AddOrUpdateVacation(EmployeeVacation item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.VacationID == 0;

        item = ent.AddOrUpdateItem<EmployeeVacation>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.VacationID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return ent.EmployeeVacations.Query().Where(s => s.VacationID == item.VacationID).Single();
      }
    }
    public void DeleteVacation(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeVacation>(id);
      }
    }


    public EmployeeVacation[] GetEmployeeVacationsByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeVacations
          .Query(EmployeeVacationIncludes.Type, new EmployeeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetMonth(month, filter).As<EmployeeDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeVacation[] GetEmployeeVacations(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = CreateModel()) {

        return ent.EmployeeVacations.Query(
          EmployeeVacationIncludes.Type | (includeEmployees ? EmployeeVacationIncludes.Employee : EmployeeVacationIncludes.None),
          new EmployeeDataCriteria() {
            StartDate = startDate,
            EndDate = endDate,
            EmployeeIDs = employeeIds,
            ListIDs = listIds,
            ItemTypeIDs = typeIds,
          }).ToArray();
      }
    }

    #endregion






    #region VacationTypes
    public EmployeeVacationType[] GetVacationTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeVacationType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeVacationType AddOrUpdateVacationType(EmployeeVacationType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeVacationType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteVacationType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeVacations.Query(new EmployeeDataCriteria() { ItemTypeIDs = id.PackArray() }).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeVacationType>(id);
      }
    }

    public bool IsVacationTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeVacations.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion
  }

}

