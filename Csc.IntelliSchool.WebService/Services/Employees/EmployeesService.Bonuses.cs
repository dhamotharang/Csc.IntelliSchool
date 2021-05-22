using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Bonuses
    public EmployeeBonus AddOrUpdateBonus(EmployeeBonus item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.BonusID == 0;

        item =  ent.AddOrUpdateItem<EmployeeBonus>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.BonusID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteBonus(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeBonus>(id);
      }
    }


    public EmployeeBonus[] GetEmployeeBonusesByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      var startDate = new DateTime(month.Year, month.Month, 1);
      var endDate = startDate.AddMonths(1).AddDays(-1);

      using (var ent = ServiceManager.CreateModel()) {
        if (filter == PeriodFilter.Current)
          return ent.EmployeeBonuses.Where(s => s.EmployeeID == employeeId && s.Date >= startDate && s.Date <= endDate).ToArray();
        else if (filter == PeriodFilter.Upcoming)
          return ent.EmployeeBonuses.Where(s => s.EmployeeID == employeeId && s.Date > endDate).ToArray();
        else if (filter == PeriodFilter.Past)
          return ent.EmployeeBonuses.Where(s => s.EmployeeID == employeeId && s.Date < startDate).ToArray();
        else
          return null;
      }
    }

    public EmployeeBonus[] GetEmployeeBonuses(DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeBonuses.Query(startDate, endDate, employeeIds, listIds, includeEmployees).ToArray();
      }
    }

    #endregion

  }

}

