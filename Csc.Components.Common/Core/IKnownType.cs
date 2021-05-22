using System;
using System.Collections.Generic;

namespace Csc.Components.Common {
  public interface IKnownType {
    IEnumerable<Type> GetKnownTypes();
  }
}