using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeMedicalClaimStatus[] MedicalClaimStatuses {
      get { return DataManager.Cache.Get<EmployeeMedicalClaimStatus[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeMedicalClaimStatus[]>();
        else
          DataManager.Cache.Add(value);
      }
    }
    private static EmployeeMedicalClaimType[] MedicalClaimTypes {
      get { return DataManager.Cache.Get<EmployeeMedicalClaimType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeMedicalClaimType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }



    #region Claims
    public static void GetMedicalClaims(int[] statusIds, DateTime? startDate, DateTime? endDate, AsyncState<EmployeeMedicalClaim[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeMedicalClaims(statusIds, startDate, endDate), callback);
    }


    public static void DeleteMedicalClaim(EmployeeMedicalClaim item, AsyncState<EmployeeMedicalClaim> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteEmployeeMedicalClaim(item.ClaimID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }

    public static void AddOrUpdateMedicalClaim(EmployeeMedicalClaim claim, AsyncState<EmployeeMedicalClaim> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployeeMedicalClaim(claim), callback);
    }
    #endregion

    #region Status list
    public static void GetMedicalClaimStatuses(bool forceLoad, AsyncState<EmployeeMedicalClaimStatus[]> callback) {
      if (forceLoad == false && MedicalClaimStatuses != null) {
        Async.OnCallback(MedicalClaimStatuses, null, callback);
        return;
      }

      MedicalClaimStatuses = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMedicalClaimStatuses(), (res, err) => {
        if (err == null) {
          MedicalClaimStatuses =
            res.Where(s => s.IsCompletion != true).OrderBy(s => s.Order).ThenBy(s => s.Name).Concat(
            res.Where(s => s.IsCompletion == true).OrderBy(s => s.Order).ThenBy(s => s.Name)).ToArray();
        }
        Async.OnCallback(MedicalClaimStatuses, err, callback);
      });
    }
    #endregion

    #region Type list
    public static void GetMedicalClaimTypes(bool forceLoad, AsyncState<EmployeeMedicalClaimType[]> callback) {
      if (forceLoad == false && MedicalClaimTypes != null) {
        Async.OnCallback(MedicalClaimTypes, null, callback);
        return;
      }

      MedicalClaimTypes = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMedicalClaimTypes(), (res, err) => {
        if (err == null) {
          MedicalClaimTypes = res.OrderBy(s => s.Name).ToArray();
        }
        Async.OnCallback(MedicalClaimTypes, err, callback);
      });
    }

    #endregion

  }
}