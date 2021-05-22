using Csc.Components.Common;
using Csc.Components.Common.Data;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region Includes
    public static DbQuery<T> Include<T>(this DbQuery<T> qry, Enum enumValues) where T : class {
      return Include<T>(qry, DataExtensions.GetIncludes(enumValues));
    }
    public static DbQuery<T> Include<T>(this DbQuery<T> qry, string[] includes) where T : class {
      Trace.WriteLine("Includes = " + string.Join(", ", includes));
      foreach (var inc in includes) {
        qry = qry.Include(inc);
      }

      return qry;
    }
    #endregion

    #region Query
    public static DbQuery<T> Query<T>(this DbQuery<T> qry, Enum enumValues = null) where T : class {
      if (enumValues != null)
        qry = qry.Include(enumValues);

      return qry;
    }

    #endregion

  }
}