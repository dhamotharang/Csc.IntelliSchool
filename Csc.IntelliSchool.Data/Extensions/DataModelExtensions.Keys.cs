using Csc.Components.Common;
using Csc.Components.Common.Data;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region Keys
    public static bool SameKeys<T>(this DbContext ent, T item1, T item2) where T : class {
      return Enumerable.SequenceEqual<int>(
        ent.GetKeys<T>(item1).Select(s => s.GetHashCode()),
        ent.GetKeys<T>(item2).Select(s => s.GetHashCode()));
    }
    public static object[] GetKeys<T>(this DbContext ent, T item) where T : class {
      ObjectContext objectContext = ((IObjectContextAdapter)ent).ObjectContext;
      ObjectSet<T> set = objectContext.CreateObjectSet<T>();
      var keyNames = set.EntitySet.ElementType.KeyMembers.Select(k => k.Name).ToArray();

      List<object> keys = new List<object>(keyNames.Count());
      Type t = item.GetType();
      foreach (var key in keyNames) {
        var prop = t.GetProperty(key);
        keys.Add(prop.GetValue(item));
      }

      return keys.ToArray();
    }
    #endregion
  }

}