using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TableDependency.Exceptions;
using TableDependency.SqlClient;

namespace Csc.Components.Data {

  public partial class TableMonitor<T> : TableMonitorBase where T : class {
    public SqlTableDependency<T> Dependency { get; private set; }

    #region Constructor
    private TableMonitor(Type type, string connStr, string tableName, TableChangeType changeTypes) : base(type, tableName, changeTypes) {
      Dependency = new SqlTableDependency<T>(connStr, tableName);
      Dependency.OnChanged += Dependency_OnChanged;
      Dependency.OnError += Dependency_OnError;
    }

    public static TableMonitor<T> Monitor(string connStr, string tableName = null, TableChangeType changeTypes = TableChangeType.All) {
      if (tableName == null) {
        Type t = typeof(T);
        var attrib = t.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
        if (attrib != null)
          tableName = attrib.Name;
      }
      if (tableName == null)
        tableName = typeof(T).Name;

      return new TableMonitor<T>(typeof(T), connStr, tableName, changeTypes);
    }
    #endregion


    public override void Start() {
      Dependency.Start();
    }
    public override void Stop() {
      if (Dependency.Status == TableDependency.Enums.TableDependencyStatus.Started)
        Dependency.Stop();
    }

    private void Dependency_OnError(object sender, TableDependency.EventArgs.ErrorEventArgs e) {
      if (e.Error is MessageMisalignedException)
        return;

      throw e.Error;
    }

    private void Dependency_OnChanged(object sender, TableDependency.EventArgs.RecordChangedEventArgs<T> e) {
      if (e.ChangeType == TableDependency.Enums.ChangeType.None)
        return;

      if ((e.ChangeType == TableDependency.Enums.ChangeType.Insert && ChangeTypes.HasFlag(TableChangeType.Insert)) ||
        (e.ChangeType == TableDependency.Enums.ChangeType.Update && ChangeTypes.HasFlag(TableChangeType.Update)) ||
        (e.ChangeType == TableDependency.Enums.ChangeType.Delete && ChangeTypes.HasFlag(TableChangeType.Delete))) {
        TableChangeType type = TableChangeType.None;
        if (e.ChangeType == TableDependency.Enums.ChangeType.Insert)
          type = TableChangeType.Insert;
        else if (e.ChangeType == TableDependency.Enums.ChangeType.Update)
          type = TableChangeType.Update;
        else if (e.ChangeType == TableDependency.Enums.ChangeType.Delete)
          type = TableChangeType.Delete;

        OnChange(new TableChangeEventArgs(e.Entity, type));
      }
    }

    public override void Dispose() {
      try {
        Stop();
      } catch {

      }
      Dependency.Dispose();
    }


  }
}
