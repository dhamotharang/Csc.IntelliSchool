using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TableDependency.SqlClient;

namespace Csc.Components.Data {
  public delegate void TableChangeEventHandler(object sender, TableChangeEventArgs e);
}
