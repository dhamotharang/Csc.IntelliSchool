using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {

    #region Documents
    public EmployeeDocument AddOrUpdateEmployeeDocument(EmployeeDocument doc, string fileUpload) {
      return null;


      //string oldDocFile = null;
      //EmployeeDocument dbItem = null;

      //if (false == string.IsNullOrEmpty(fileUpload)) {
      //  doc.Url = UploadFile(fileUpload, AppPathSection.HumanResourcesDocuments);
      //}

      //using (var ent = CreateModel()) {

      //  if (doc.DocumentID == 0) {
      //    ent.EmployeeDocuments.Add(doc);
      //    dbItem = doc;
      //  } else {
      //    dbItem = ent.EmployeeDocuments.Find(doc.DocumentID);
      //    oldDocFile = dbItem.Url;

      //    UpdateEntity(ent, dbItem, doc);
      //  }

      //  ent.SaveChanges();

      //  ent.Logger.LogDatabase(CurrentUser, dbItem == null ? SystemLogDataAction.Insert : SystemLogDataAction.Update, typeof(EmployeeDocument), doc.DocumentID.PackArray(),
      //    new SystemLogEmployeeEntry(doc.EmployeeID));
      //  ent.Logger.Flush();

      //}

      //if (string.IsNullOrEmpty(oldDocFile) == false && (doc.Url == null || string.Compare(oldDocFile, doc.Url, true) != 0)) {
      //  try {
      //    DeleteFile(oldDocFile, AppPathSection.HumanResourcesDocuments);
      //  } catch (Exception ex) {
      //    using (var ent = CreateModel()) {
      //      ent.Logger.LogError(CurrentUser, SystemLogApplicationAction.Files, ex);
      //      ent.Logger.Flush();
      //    }
      //  }
      //}

      //return dbItem;
    }

    public EmployeeDocument[] GetEmployeeDocuments(int employeeId) {
      using (var ent = CreateModel()) {
        return ent.EmployeeDocuments.Where(s => s.EmployeeID == employeeId).ToArray();
      }
    }


    public void DeleteDocument(int id) {
      using (var ent = CreateModel()) {
        ent.RemoveItem<EmployeeDocument>(id);
      }
    }
    #endregion
  }

}

