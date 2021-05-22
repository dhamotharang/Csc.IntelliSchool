using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Csc.IntelliSchool.Modules.HRModule.Views.Earning {
 public interface IEarningControl {
    EmployeeEarningSection Section { get; }
    DateTime? PickedMonth { get; set; }
    bool HasUpdates { get; }

    void OnLoadData(bool force = false);
  }
}
