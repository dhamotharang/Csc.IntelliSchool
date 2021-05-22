using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Csc.IntelliSchool.WebService.Services.SystemAdmin {
  public partial class SystemAdminService {
    public SystemLog[] GetSystemLog(DateTime start, DateTime end) {
      start = start.ToUniversalTime();
      end = end.ToUniversalTime();
      using (var ent = ServiceManager.CreateModel()) {
        return ent.SystemLogs.Include("Data").Where(s => s.DateTimeUtc >= start && s.DateTimeUtc <= end).ToArray();
      }
    }
  }
}
