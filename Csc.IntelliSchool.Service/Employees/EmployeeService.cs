using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService : DataService {
    private static object _lockObject = new object();
    private static EmployeesService _service;

    public static EmployeesService Instance {
      get {
        if (_service == null)
          lock (_lockObject)
            if (_service == null)
              _service = new EmployeesService();
        return _service;
      }
    }

    private EmployeesService() {
    }
  }
}