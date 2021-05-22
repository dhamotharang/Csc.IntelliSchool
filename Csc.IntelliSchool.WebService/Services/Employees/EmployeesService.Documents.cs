using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial class EmployeesService : IEmployeesService {

    #region Documents
    public EmployeeDocument AddOrUpdateEmployeeDocument(EmployeeDocument doc, string fileUpload) {
      return null;


      //string oldDocFile = null;
      //EmployeeDocument dbItem = null;

      //if (false == string.IsNullOrEmpty(fileUpload)) {
      //  doc.Url = UploadFile(fileUpload, AppPathSection.HumanResourcesDocuments);
      //}

      //using (var ent = ServiceManager.CreateModel()) {

      //  if (doc.DocumentID == 0) {
      //    ent.EmployeeDocuments.Add(doc);
      //    dbItem = doc;
      //  } else {
      //    dbItem = ent.EmployeeDocuments.Find(doc.DocumentID);
      //    oldDocFile = dbItem.Url;

      //    UpdateEntity(ent, dbItem, doc);
      //  }

      //  ent.SaveChanges();

      //  ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), dbItem == null ? SystemLogDataAction.Insert : SystemLogDataAction.Update, typeof(EmployeeDocument), doc.DocumentID.PackArray(),
      //    new SystemLogEmployeeEntry(doc.EmployeeID));
      //  ent.Logger.Flush();

      //}

      //if (string.IsNullOrEmpty(oldDocFile) == false && (doc.Url == null || string.Compare(oldDocFile, doc.Url, true) != 0)) {
      //  try {
      //    DeleteFile(oldDocFile, AppPathSection.HumanResourcesDocuments);
      //  } catch (Exception ex) {
      //    using (var ent = ServiceManager.CreateModel()) {
      //      ent.Logger.LogError(ServiceManager.GetCurrentUser(ent), SystemLogApplicationAction.Files, ex);
      //      ent.Logger.Flush();
      //    }
      //  }
      //}

      //return dbItem;
    }

    public EmployeeDocument[] GetEmployeeDocuments(int employeeId) {
      using (var ent = ServiceManager.CreateModel()) {
        return ent.EmployeeDocuments.Where(s => s.EmployeeID == employeeId).ToArray();
      }
    }


    public void DeleteDocument(int id) {
      using (var ent = ServiceManager.CreateModel()) {
        ent.RemoveItem<EmployeeDocument>(id);
      }
    }
    #endregion
  }

}

