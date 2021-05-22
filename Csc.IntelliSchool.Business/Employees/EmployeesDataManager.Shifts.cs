
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Shifts
    public static void AddOrUpdateShift(EmployeeShift item, AsyncState<EmployeeShift> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateShift(item), callback);
    }

    public static void DeleteShift(EmployeeShift item, AsyncState<EmployeeShift> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteShift(item.ShiftID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }


    public static void GetShifts( AsyncState<EmployeeShift[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetShifts(), callback);
    }
    #endregion

    #region ShiftOverrides
    public static void AddOrUpdateShiftOverride(EmployeeShiftOverride item, AsyncState<EmployeeShiftOverride> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateShiftOverride(item), callback);
    }

    public static void DeleteShiftOverride(EmployeeShiftOverride item, AsyncState<EmployeeShiftOverride> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteShiftOverride(item.OverrideID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void GetShiftOverrides(int? shiftId, int[] typeIds,  int? year, AsyncState<EmployeeShiftOverride[]> callback) {
      DateTime? startDate = null, endDate = null;
      if (year != null) {
        startDate = new DateTime(year.Value, 1, 1);
        endDate = startDate.ToYearEnd();
      }
      GetShiftOverrides(shiftId, typeIds, startDate, endDate, callback);
    }

    public static void GetShiftOverrides(int? shiftId,int[] typeIds, DateTime? startDate, DateTime? endDate , AsyncState<EmployeeShiftOverride[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetShiftOverrides(shiftId, typeIds, startDate, endDate), callback);
    }

    #endregion

    #region ShiftOverrideTypes
    public static void AddOrUpdateShiftOverrideType(EmployeeShiftOverrideType item, AsyncState<EmployeeShiftOverrideType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateShiftOverrideType(item), callback);
    }

    public static void DeleteShiftOverrideType(EmployeeShiftOverrideType item, AsyncState<EmployeeShiftOverrideType> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteShiftOverrideType(item.TypeID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }


    public static void GetShiftOverrideTypes(AsyncState<EmployeeShiftOverrideType[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetShiftOverrideTypes(), callback);
    }
    #endregion

  }
}