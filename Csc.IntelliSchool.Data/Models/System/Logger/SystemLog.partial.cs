
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {

  public partial class SystemLog : IChildrenObject {
    [IgnoreDataMember]
    [NotMapped]
    public object[] ChildObjects { get { return Data != null ? Data.ToArray() : new object[] { }; } }


    [IgnoreDataMember]
    [NotMapped]
    public DateTime? DateTime {
      get { return DateTimeUtc != null ? DateTimeUtc.Value.ToLocalTime() : new DateTime?(); }
      set { DateTimeUtc = value != null ? value.Value.ToUniversalTime() : new DateTime?(); }

    }

    [IgnoreDataMember]
    [NotMapped]
    public string FullUsername {
      get {
        string str = Username;

        if (str == null)
          return null;

        if (str.Length > 0 && UserID != null)
          str += string.Format(" {0}", UserID);

        return str;

      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public SystemLogCategory LogCategory {
      get {
        SystemLogCategory cat = SystemLogCategory.Unknown;
        Enum.TryParse<SystemLogCategory>(Category, true, out cat);
        return cat;
      }
      set {
        Category = value.ToString();
      }
    }

    [IgnoreDataMember]
    [NotMapped]
    public SystemLogLevel LogLevel {
      get {
        SystemLogLevel lvl = SystemLogLevel.Unknown;
        Enum.TryParse<SystemLogLevel>(Level, true, out lvl);
        return lvl;
      }
      set {
        Level = value.ToString();
      }
    }
  }

}