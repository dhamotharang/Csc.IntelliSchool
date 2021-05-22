
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeList[] Lists {
      get { return DataManager.Cache.Get<EmployeeList[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeList[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    #region Lists
    public static void GetLists(AsyncState<EmployeeList[]> callback /*, bool includeNullItem = false */) {
      Action<IEnumerable<EmployeeList>, Exception> locCallback = (res, err) => {
        var items = res;
        //if (includeNullItem && items != null)
        //  items = new EmployeeList[] { new EmployeeList() { Name = Properties.Resources.Text_Unlisted } }.Concat(items).ToArray();

        Async.OnCallback(items != null ? items.ToArray() : null, err, callback);
      };

      Lists = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetLists(), (res, err) => {
        if (err == null)
          Lists = res.OrderBy(s => s.Name).ToArray();
        locCallback(Lists, err);
      });
    }
    #endregion
  }
}