
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeePosition[] Positions {
      get { return DataManager.Cache.Get<EmployeePosition[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeePosition[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    #region Positions
    public static void GetPositions(bool forceLoad, int? listId, AsyncState<EmployeePosition[]> callback) {
      Action<IEnumerable<EmployeePosition>, Exception> locCallback = (res, err) => {
        Async.OnCallback(res != null ?
        res.Where(s => s.Lists.Any(x => x.ListID == listId)).OrderBy(s => s.Name).ToArray()
          .Concat(res.Where(s => s.Lists.Any(x => x.ListID == listId) == false).OrderBy(s => s.Name).ToArray())
          .ToArray()
          : null, err, callback);
      };


      if (forceLoad == false && Positions != null) {
        locCallback(Positions, null);
        return;
      }

      Positions = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetPositions(), (res, err) => {
        if (err == null)
          Positions = res.OrderBy(s => s.Name).ToArray();
        locCallback(Positions, err);
      });
    }
    public static void DeletePosition(EmployeePosition item, AsyncState<EmployeePosition> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeletePosition(item.PositionID), (err) => {
        if (err == null)
          Positions = Positions.Except(new[] { item }).ToArray();
        Async.OnCallback(err == null ? item : null, err, callback);
      });
    }
    public static void AddPosition(EmployeePosition item, AsyncState<EmployeePosition> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdatePosition(item), (res, err) => {
        if (err == null)
          Positions = Positions.Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        Async.OnCallback(res, err, callback);
      });
    }
    public static void UpdatePosition(EmployeePosition item, AsyncState<EmployeePosition> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdatePosition(item), (res, err) => {
        if (err == null) {
          Positions = Positions.Except(new[] { item }).Concat(new[] { res }).OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(res, err, callback);
      });
    }
    #endregion
  }
}