using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Deductions
    public EmployeeDeduction AddOrUpdateDeduction(EmployeeDeduction item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.DeductionID == 0;

        item = ent.AddOrUpdateItem<EmployeeDeduction>(item);

        ent.Logger.LogDatabase(CurrentUser,
                isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
                item.DeductionID.PackArray(),
                new SystemLogDataEntry() { EmployeeID = item.EmployeeID }, null);
        ent.SaveChanges();

        return ent.EmployeeDeductions.Query(EmployeeDeductionIncludes.Type).Where(s => s.DeductionID == item.DeductionID).Single();
      }
    }
    public void DeleteDeduction(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeDeduction>(id);
      }
    }


    public EmployeeDeduction[] GetEmployeeDeductionsByPeriod(int employeeId, DateTime month, PeriodFilter filter) {
      using (var ent = CreateModel()) {
        return ent.EmployeeDeductions
          .Query(EmployeeDeductionIncludes.Type,
          new EmployeeDataCriteria() { EmployeeIDs = employeeId.PackArray() }.SetMonth(month, filter).As<EmployeeDataCriteria>())
          .ToArray();
      }
    }

    public EmployeeDeduction[] GetEmployeeDeductions(int[] typeIds, DateTime startDate, DateTime endDate, int[] employeeIds, int[] listIds, bool includeEmployees) {
      startDate = startDate.ToDay();
      endDate = endDate.ToDay();

      using (var ent = CreateModel()) {

        return ent.EmployeeDeductions.Query(
          EmployeeDeductionIncludes.Type | (includeEmployees ? EmployeeDeductionIncludes.Employee : EmployeeDeductionIncludes.None),
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


    #region DeductionTypes
    public EmployeeDeductionType[] GetDeductionTypes() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeDeductionType>().OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeDeductionType AddOrUpdateDeductionType(EmployeeDeductionType item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.TypeID == 0;

        item = ent.AddOrUpdateItem<EmployeeDeductionType>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.TypeID.PackArray(),
          new SystemLogDataEntry() { Name = item.Name }, null);
        ent.SaveChanges();

        return item;
      }
    }
    public void DeleteDeductionType(int id) {
      using (var ent = CreateModel()) {
        var linkedItems = ent.EmployeeBonuses.Query(new EmployeeDataCriteria() { ItemTypeIDs = id.PackArray() }).ToArray();
        foreach (var itm in linkedItems) {
          itm.TypeID = null;
        }

        ent.RemoveItem<EmployeeDeductionType>(id);
      }
    }

    public bool IsDeductionTypeUsed(int id) {
      using (var ent = CreateModel()) {
        return ent.EmployeeDeductions.Where(s => s.TypeID == id).Count() > 0;
      }
    }
    #endregion

  }

}

