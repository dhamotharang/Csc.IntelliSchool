using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {
    #region Lists
    public EmployeeList[] GetLists() {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.GetItems<EmployeeList>().OrderBy(s => s.Name).ToArray();
      }
    }
    #endregion
  }

}

