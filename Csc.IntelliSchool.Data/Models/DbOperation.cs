using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;

namespace Csc.IntelliSchool.Data {
  [Flags]
  public enum DbOperation {
    None = 0,
    Add = 1 << 0,
    Update = 1 << 1,
    Delete = 1 << 2,
    All = Add | Update | Delete
  }
}
