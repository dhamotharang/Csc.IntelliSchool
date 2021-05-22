using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    #region Departments
    public EmployeeDepartment[] GetDepartments() {
      using (var ent = CreateModel()) {
        return ent.GetItems<EmployeeDepartment>("Lists").OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeDepartment AddOrUpdateDepartment(EmployeeDepartment item) {
      using (var ent = CreateModel()) {
        bool isInsert = item.DepartmentID == 0;

        item = ent.AddOrUpdateItem<EmployeeDepartment>(item);

        ent.Logger.LogDatabase(CurrentUser,
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.DepartmentID.PackArray(),
          new SystemLogDataEntry() {  Name = item.Name }, null);
              ent.SaveChanges();

        return item;
      }
    }
    public void DeleteDepartment(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeDepartment>(id);
      }
    }
    #endregion
  }

}

