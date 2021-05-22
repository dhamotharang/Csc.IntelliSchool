using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class EmployeesDataManager {
    #region Documents
    public static void AddOrUpdateDocument(EmployeeDocument doc, string fileUpload, AsyncState<EmployeeDocument> callback) {
      //Async.AsyncCall(() => {
      //  if (fileUpload != null)
      //    doc.Url = FileManager.UploadFileSync(fileUpload, Services.FileHandler.RemoteFileType.EmployeeDocument);

      //  return Service.EmployeesService.Instance.AddOrUpdateEmployeeDocument(doc, fileUpload);

      //}, callback);
    }

    public static void GetDocuments(int employeeId, AsyncState<EmployeeDocument[]> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.GetEmployeeDocuments(employeeId), callback);
    }


    public static void DeleteDocument(EmployeeDocument item, AsyncState<EmployeeDocument> callback) {
      Async.AsyncCall(() => Service.EmployeesService.Instance.DeleteLoan(item.DocumentID), (err) => Async.OnCallback(err == null ? item : null, err, callback));
    }
    #endregion
  }
}