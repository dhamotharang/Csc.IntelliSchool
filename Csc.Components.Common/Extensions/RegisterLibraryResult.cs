using System;

namespace Csc.Components.Common {
  public class RegisterLibraryResult {
    public RegisterLibraryStatus Status { get; private set; }
    public string Library { get; private set; }
    public Exception Error { get; set; }

    public RegisterLibraryResult() { }
    public RegisterLibraryResult(string lib, RegisterLibraryStatus status) {
      this.Library = lib;
      this.Status = status;
    }
    public RegisterLibraryResult(string lib, Exception error) {
      this.Library = lib;
      this.Error = error;
    }
  }
}