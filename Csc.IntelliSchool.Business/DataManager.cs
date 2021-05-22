using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Business {
  public static partial class DataManager {
    private static Csc.Components.Common.CacheManager _cache;

    public static Csc.Components.Common.CacheManager Cache {
      get {
        if (_cache == null)
          lock (typeof(DataManager))
            if (_cache == null)
              _cache = new Components.Common.CacheManager(typeof(DataManager).FullName);
        return _cache;
      }
    }
    public static User CurrentUser { get; internal set; }


    //private static Services.Employees.EmployeesServiceClient _employeesService = null;
    //private static Services.SystemAdmin.SystemAdminServiceClient _systemService = null;
    //private static Services.CommonData.PeopleServiceClient _commonService = null;
    //private static Services.Account.AccountServiceClient _accountService = null;
    //private static Services.FileHandler.FileHandlerServiceClient _fileHandlerService = null;
    //private static Csc.Components.Services.CustomEndpointBehavior _behavior;

    //internal static Services.Employees.EmployeesServiceClient EmployeesService
    //{
    //  get
    //  {
    //    if (_employeesService == null)
    //      lock (typeof(DataManager))
    //        if (_employeesService == null) {
    //          _employeesService = new Services.Employees.EmployeesServiceClient();
    //          _employeesService.Endpoint.EndpointBehaviors.Add(EndpointBehavior);
    //        }

    //    return _employeesService;
    //  }
    //}
    //internal static Services.SystemAdmin.SystemAdminServiceClient SystemService
    //{
    //  get
    //  {
    //    if (_systemService == null)
    //      lock (typeof(DataManager))
    //        if (_systemService == null) {
    //          _systemService = new Services.SystemAdmin.SystemAdminServiceClient();
    //          _systemService.Endpoint.EndpointBehaviors.Add(EndpointBehavior);
    //        }

    //    return _systemService;
    //  }
    //}
    //internal static Services.CommonData.PeopleServiceClient PeopleService
    //{
    //  get
    //  {
    //    if (_commonService == null)
    //      lock (typeof(DataManager))
    //        if (_commonService == null) {
    //          _commonService = new Services.CommonData.PeopleServiceClient();
    //          _commonService.Endpoint.EndpointBehaviors.Add(EndpointBehavior);
    //        }

    //    return _commonService;
    //  }
    //}
    //internal static Services.Account.AccountServiceClient AccountService
    //{
    //  get
    //  {
    //    if (_accountService == null)
    //      lock (typeof(DataManager))
    //        if (_accountService == null) {
    //          _accountService = new Services.Account.AccountServiceClient();
    //          _accountService.Endpoint.EndpointBehaviors.Add(EndpointBehavior);
    //        }

    //    return _accountService;
    //  }
    //}
    //internal static Services.FileHandler.FileHandlerServiceClient FileHandlerService
    //{
    //  get
    //  {
    //    if (_fileHandlerService == null)
    //      lock (typeof(DataManager))
    //        if (_fileHandlerService == null) {
    //          _fileHandlerService = new Services.FileHandler.FileHandlerServiceClient();
    //          _fileHandlerService.Endpoint.EndpointBehaviors.Add(EndpointBehavior);
    //        }

    //    return _fileHandlerService;
    //  }
    //}
    //private static Csc.Components.Services.CustomEndpointBehavior EndpointBehavior
    //{
    //  get
    //  {
    //    if (_behavior == null)
    //      lock (typeof(DataManager))
    //        if (_behavior == null)
    //          _behavior = new Components.Services.CustomEndpointBehavior() {
    //            HeadersCallback = FillRequestHeaders,
    //            MaxItemsInObjectGraph = int.MaxValue
    //          };

    //    return _behavior;
    //  }
    //}

    //private static IDictionary<string, string> FillRequestHeaders() {
    //  Dictionary<string, string> dict = new Dictionary<string, string>();
    //  if (Service.AccountService.CurrentUser != null)
    //    dict.Add("UserID", Service.AccountService.CurrentUser.UserID.ToString());

    //  return dict;
    //}




  }
}