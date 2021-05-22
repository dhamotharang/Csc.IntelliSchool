using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;


using System.Text;

namespace Csc.IntelliSchool.Service {
  public partial class SystemService {
    public SystemLog[] GetSystemLog(DateTime start, DateTime end) {
      start = start.ToUniversalTime();
      end = end.ToUniversalTime();
      using (var ent = CreateModel()) {
        return ent.SystemLogs.Include("Data").Where(s => s.DateTimeUtc >= start && s.DateTimeUtc <= end).ToArray();
      }
    }
  }
}
