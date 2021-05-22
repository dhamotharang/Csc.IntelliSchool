using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Security {
  public static class Cryptography {
    public static string HashSha1(string inputString) {
      using (var sha1 = new System.Security.Cryptography.SHA1Managed()) {
        byte[] textData = Encoding.UTF8.GetBytes(inputString);
        byte[] hash = sha1.ComputeHash(textData);

        return System.Convert.ToBase64String(hash);
      }
    }

  }


}
