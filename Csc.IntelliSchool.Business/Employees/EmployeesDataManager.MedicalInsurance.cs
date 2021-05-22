
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    private static EmployeeMedicalProgram[] MedicalPrograms {
      get { return DataManager.Cache.Get<EmployeeMedicalProgram[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<EmployeeMedicalProgram[]>();
        else
          DataManager.Cache.Add(value);
      }
    }

    #region Programs
    public static void GetMedicalPrograms(bool forceLoad, AsyncState<EmployeeMedicalProgram[]> callback) {
      Action<IEnumerable<EmployeeMedicalProgram>, Exception> locCallback = (res, err) => {
        if (res != null)
          res = res.OrderBy(s => s.FullName);
        Async.OnCallback(res != null ? res.ToArray() : null, err, callback);
      };

      if (forceLoad == false && MedicalPrograms != null) {
        locCallback(MedicalPrograms, null);
        return;
      }

      MedicalPrograms = null;
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeMedicalPrograms(EmployeeMedicalProgramDataFilter.Full), (res, err) => {
        if (err == null) {
          MedicalPrograms = res.OrderBy(s => s.FullName).ToArray();
        }
        locCallback(MedicalPrograms, err);
      });
    }
    #endregion

    public static void GetApplicableMedicalEmployees(AsyncState<Employee[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetApplicableMedicalEmployees(EmployeeIncludes.MedicalList), callback);
    }
    //public static void GetAllEmployeeData(bool? active, AsyncState<Employee[]> callback) {
    //  Async.AsyncCall(() => Handler.GetAllEmployeeData(active), callback);
    //}
    //public static void GetAllEmployeeData(int employeeId, AsyncState<Employee> callback) {
    //  Async.AsyncCall(() => Handler.GetAllEmployeeData(employeeId), callback);
    //}

    #region Updates
    public static void ApplyMedicalSalaries(int[] employeeIds, bool applySalaries, DateTime? applyMonth, AsyncState<Employee[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.ApplyEmployeeMedicalSalaries(employeeIds, applySalaries, applyMonth), callback);
    }
    #endregion

    #region Requests
    public static void GetProgramTemplate(EmployeeMedicalRequest req, AsyncState<EmployeeMedicalProgramTemplate> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetMedicalProgramTemplate(req), callback);
    }

    public static void GenerateMedicalRequest(EmployeeMedicalRequest req, string filename, AsyncState<string> callback) {
      Async.AsyncCall<string>(() => {
        using (var memStm = Service.EmployeesService.Instance.GenerateEmployeeMedicalRequest(req))
        using (var fileStm = new FileStream(filename, FileMode.Create)) {
          byte[] data = new byte[memStm.Length];
          memStm.Read(data, 0, data.Length);
          fileStm.Write(data, 0, data.Length);
        }
        return filename;
      }, callback);
    }
    #endregion



  }
}