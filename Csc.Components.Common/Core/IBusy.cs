using System;

namespace Csc.Components.Common {
  public interface IBusy {
    event EventHandler BusyChanged;
    bool IsBusy { get; }
    void SetBusy(bool busy);
  }
}