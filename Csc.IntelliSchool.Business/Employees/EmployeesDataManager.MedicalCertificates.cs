
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {

    #region Certificates
    public static void AddOrUpdateMedicalCertificate(EmployeeMedicalCertificate item, AsyncState<EmployeeMedicalCertificate> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.AddOrUpdateEmployeeMedicalCertificate(item), callback);
    }
    public static void CheckMedicalCodeUsed(EmployeeMedicalCertificate cert, AsyncState<bool> callback) {
      Async.AsyncCall(() =>
        Service.EmployeesService.Instance.CheckEmployeeMedicalCodeUsed(
          cert.CertificateOwner == EmployeeMedicalCertificateOwner.Dependant ? new int[] { cert.CertificateID, ((EmployeeDependant)cert.OwnerObject).Employee.MedicalCertificateID.Value } : new int[] { cert.CertificateID },
          MedicalPrograms.Single(s => s.ProgramID == cert.ProgramID).ProviderID, cert.Code),
        callback);
    }



    public static void DeleteMedicalCertificate(EmployeeMedicalCertificate item, AsyncState<EmployeeMedicalCertificate> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteEmployeeMedicalCertificate(item.CertificateID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }
    #endregion



    #region Recalculate
    public static void RecalculateMedicalCertificates(int[] certificateIds, bool recalcRates, bool recalcCertificates, bool excludeCustomCertificates, AsyncState callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.RecalculateEmployeeMedicalCertificates(certificateIds, recalcRates, recalcCertificates, excludeCustomCertificates), callback);
    }
    #endregion

    #region Generation
    public static void GenerateMedicalCertificates(int[] employeeIds, int[] programIds, AsyncState<EmployeeMedicalCertificateGroup[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GenerateEmployeeMedicalCertificates(employeeIds, programIds), callback);
    }

    public static void GenerateMedicalProgramCertificates(int[] programIds, int[] employeeIds, AsyncState<Employee[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GenerateEmployeeMedicalProgramCertificates(programIds, employeeIds), callback);
    }
    #endregion

  }
}