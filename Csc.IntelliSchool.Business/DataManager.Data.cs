
//using Csc.Components.Common; using Csc.IntelliSchool.Data;

//namespace Csc.IntelliSchool.Business {
//  public static partial class DataManager {
//    public static void UploadEmployeePhoto(string filename, AsyncState<string> callback) {
//      AsyncUploadPhoto(filename, AppPathSection.HumanResourcesPhotos, true, callback);
//    }
//    public static void UploadPhoto(string filename, AppPathSection section, bool autoResize, AsyncState<string> callback) {
//      Async.AsyncCall(() => Handler.UploadPhoto(filename, section, autoResize), callback);
//    }


//    public static void DeleteEmployeePhotos(string[] files, AsyncState callback) {
//      Async.AsyncCall(() => {
//        foreach (var filename in files)
//          Handler.DeleteFile(filename, AppPathSection.HumanResourcesPhotos);
//      }, callback);
//    }
//    public static void DeleteFile(string filename, AppPathSection section, AsyncState callback) {
//      Async.AsyncCall(() => Handler.DeleteFile(filename, section), callback);
//    }

//    //public static void UploadFile(string filename, AppPathSection section, AsyncState<string> callback) {
//    //  Async.AsyncCall(() => Handler.UploadFile(filename, section), callback);
//    //}

//    //public static void UploadEmployeeDocument(string filename, AsyncState<string> callback) {
//    //  Async.AsyncCall(() => Handler.UploadFile(filename, AppPathSection.HumanResourcesDocuments), callback);
//    //}
//  }
//}