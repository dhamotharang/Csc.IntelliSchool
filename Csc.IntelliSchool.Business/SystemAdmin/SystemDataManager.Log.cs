
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;

namespace Csc.IntelliSchool.Business {
  public static partial class SystemDataManager {
    public static void GetSystemLog(DateTime start, DateTime end, AsyncState<SystemLog[]> callback) {
      Async.AsyncCall(() => Service.SystemService.Instance.GetSystemLog(start, end), callback);
    }


  }
}