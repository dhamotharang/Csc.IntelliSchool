using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq.Expressions;

namespace Csc.Components.Common.Data {
  public class DynObject : DynamicObject, INotifyPropertyChanged {
    private IDictionary<string, object> _data;

    protected IDictionary<string, object> Data { get { return _data; } private set { _data = value; OnPropertyChanged(() => Data); } }

    public DynObject() { Data = new Dictionary<string, object>(); }
    public DynObject(IDictionary<string, object> data) { Data = data; }


    public static string GetColumnHeader(string columnName) {
      columnName = columnName.Trim('$');
      if (columnName.Contains("##"))
        columnName = columnName.Substring(0, columnName.IndexOf("##"));
      return columnName;
    }
    public static DynObjectColumnFlags GetColumnFlags(string columnName) {
      DynObjectColumnFlags flags = DynObjectColumnFlags.None;
      if (columnName.StartsWith("$$"))
        flags |= DynObjectColumnFlags.Header;
      if (columnName.EndsWith("$$"))
        flags |= DynObjectColumnFlags.Hidden;

      if (columnName.Contains("##")) {
        var typeName = columnName.TrimEnd('$').Substring(columnName.IndexOf("##") + 2);
        DynObjectColumnFlags typeFlag = DynObjectColumnFlags.None;
        if (Enum.TryParse<DynObjectColumnFlags>(typeName, out typeFlag))
          flags |= typeFlag;
      }

      return flags;
    }

    public static string FormatColumnName(string columnName, DynObjectColumnFlags flags = DynObjectColumnFlags.None) {
      if (flags.HasFlag(DynObjectColumnFlags.Header))
        columnName = "$$" + columnName;

      if (flags.HasFlag(DynObjectColumnFlags.Boolean))
        columnName += "##Boolean";
      else if (flags.HasFlag(DynObjectColumnFlags.TrueFalse))
        columnName += "##TrueFalse";
      else if (flags.HasFlag(DynObjectColumnFlags.Date))
        columnName += "##Date";
      else if (flags.HasFlag(DynObjectColumnFlags.Text))
        columnName += "##Text";
      else if (flags.HasFlag(DynObjectColumnFlags.Number))
        columnName += "##Number";

      if (flags.HasFlag(DynObjectColumnFlags.Hidden))
        columnName += "$$";

      return columnName;
    }


    public object this[string columnName] {
      get {
        if (Data.ContainsKey(columnName))
          return Data[columnName];

        return null;
      }
      set {
        if (!Data.ContainsKey(columnName)) {
          Data.Add(columnName, value);

          OnPropertyChanged(columnName);
        } else if (Data[columnName] != value) {
          Data[columnName] = value;
          OnPropertyChanged(columnName);
        }
      }
    }


    public override IEnumerable<string> GetDynamicMemberNames() { return Data.Keys; }
    public override bool TryGetMember(GetMemberBinder binder, out object result) { result = this[binder.Name]; return true; }
    public override bool TrySetMember(SetMemberBinder binder, object value) { this[binder.Name] = value; return true; }


    #region Property Change Notify
    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged<T>(Expression<Func<T>> expr) { this.OnPropertyChanged(((MemberExpression)expr.Body).Member.Name); }
    protected void OnPropertyChanged(string propName) { if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propName)); }
    #endregion
  }
}