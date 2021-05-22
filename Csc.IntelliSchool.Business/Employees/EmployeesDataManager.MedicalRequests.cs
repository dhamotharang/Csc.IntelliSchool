
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeMedicalRequestType[] MedicalRequestTypes {
      get { return DataManager.Cache.Get<EmployeeMedicalRequestType[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeMedicalRequestType[]>();
        else
          DataManager.Cache.Add(value);
      }
    }


    public static void GetMedicalRequestTypes(bool forceLoad, AsyncState<EmployeeMedicalRequestType[]> callback) {
      Action<IEnumerable<EmployeeMedicalRequestType>, Exception> locCallback = (res, err) => {
        if (res != null)
          res = res.OrderBy(s => s.Name);
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && MedicalPrograms != null) {
        locCallback(MedicalRequestTypes, null);
        return;
      }

      MedicalPrograms = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMedicalRequestTypes(), (res, err) => {
        if (err == null) {
          MedicalRequestTypes = res.OrderBy(s => s.Name).ToArray();
        }
        locCallback(MedicalRequestTypes, err);
      });
    }




  }
}