using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Departments
    public EmployeeDepartment[] GetDepartments() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeDepartment>("Lists").OrderBy(s => s.Name).ToArray();
      }
    }
    public EmployeeDepartment AddOrUpdateDepartment(EmployeeDepartment item) {
      using (var ent = ServiceManager.CreateModel()) {
        bool isInsert = item.DepartmentID == 0;

        item = ent.AddOrUpdateItem<EmployeeDepartment>(item);

        ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent),
          isInsert ? SystemLogDataAction.Insert : SystemLogDataAction.Update, item.GetType(),
          item.DepartmentID.PackArray(),
          new SystemLogDataEntry() {  Name = item.Name }, null);
              ent.SaveChanges();

        return item;
      }
    }
    public void DeleteDepartment(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeDepartment>(id);
      }
    }
    #endregion
  }

}

