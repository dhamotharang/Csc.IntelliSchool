using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TableDependency.SqlClient;

namespace Csc.Components.Data {
  public class TableChangeEventArgs {
    public object Entity { get; set; }
    public TableChangeType ChangeType { get; set; }

    public TableChangeEventArgs() { }
    public TableChangeEventArgs(object entity, TableChangeType change) {
      Entity = entity;
      ChangeType = change;
    }



  }
}
