using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.Components.Common {
  public interface ILock {
    Object LockObject { get; }
  }
}