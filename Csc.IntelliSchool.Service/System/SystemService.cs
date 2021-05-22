using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Text;

namespace Csc.IntelliSchool.Service {
  public partial class SystemService : DataService {
    private static object _lockObject = new object();
    private static SystemService _service;

    public static SystemService Instance {
      get {
        if (_service == null)
          lock (_lockObject)
            if (_service == null)
              _service = new SystemService();
        return _service;
      }
    }

    private SystemService() {

    }
  }
}
