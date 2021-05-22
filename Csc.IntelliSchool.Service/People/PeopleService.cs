using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Service {
  public partial class PeopleService : DataService {
    private static object _lockObject = new object();
    private static PeopleService _service;

    public static PeopleService Instance {
      get {
        if (_service == null)
          lock (_lockObject)
            if (_service == null)
              _service = new PeopleService();
        return _service;
      }
    }

    private PeopleService() {

    }
  }

}

