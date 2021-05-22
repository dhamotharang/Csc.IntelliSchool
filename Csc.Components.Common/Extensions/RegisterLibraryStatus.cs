using Csc.Components.Common;
using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Csc.Components.Common {

  public enum RegisterLibraryStatus {
    Success = 0,
    InvalidArguments,
    OleInitialize,
    LoadLibrary,
    GetProcAddress,
    RegisterServer,
  }
}