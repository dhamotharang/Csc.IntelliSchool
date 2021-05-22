using System.DirectoryServices.AccountManagement;

namespace Csc.Components.Security {
  public static class PrincipalManagement {
    public static bool ValidateWindows(string username, string password) {
      return ValidateWindows(null, username, password);
    }

    public static bool ValidateWindows(string domain, string username, string password) {
      using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain)) {
        return pc.ValidateCredentials(username, password);
      }
    }
  }
}
