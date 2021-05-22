using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TableDependency.SqlClient;

namespace Csc.Components.Data {
  public abstract class TableMonitorBase : IDisposable {
    public event TableChangeEventHandler TableChanged;
    public Type Type { get; private set; }
    public string TableName { get; private set; }
    public TableChangeType ChangeTypes { get; set; }


    public TableMonitorBase(Type type, string tableName, TableChangeType changeTypes = TableChangeType.All) {
      this.Type = type;
      this.TableName = tableName;
      this.ChangeTypes = changeTypes;
    }



    public abstract void Start();
    public abstract void Stop();

    public abstract void Dispose();

    protected virtual void OnChange(TableChangeEventArgs e) {
      if (TableChanged != null)
        TableChanged(this, e);
    }
  }
}
