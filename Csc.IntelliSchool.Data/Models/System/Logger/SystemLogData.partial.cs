
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class SystemLogData {
    public SystemLogData() {
    }
    public SystemLogData(string prop, object value) {
      Property = prop;
      Value = value != null ? value.ToString() : null;
    }
  }


}