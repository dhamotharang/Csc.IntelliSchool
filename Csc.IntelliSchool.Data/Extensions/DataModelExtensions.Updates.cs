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
    #region Updates
    public static void UpdateEntity<T>(this DataEntities ent, T dbItem, T userItem) where T : class {
      ent.Entry(dbItem).CurrentValues.SetValues(userItem);
    }

    public static void UpdateRelatedEntity<T>(this DataEntities ent, T dbItem, T userItem) where T : class {
      //if (userItem == null && dbItem != null)
      //  ent.Set<T>().Remove(dbItem);
      //else 
      
      if (userItem != null && dbItem == null)
        ent.Set<T>().Add(userItem);
      else if (userItem != null && dbItem != null) {
        if (ent.SameKeys(dbItem, userItem))
          ent.Entry(dbItem).CurrentValues.SetValues(userItem);
        else {
          ent.Set<T>().Remove(dbItem);
          ent.Set<T>().Add(userItem);
        }
      }
    }
    public static void UpdateChildEntities<T>(this DataEntities ent, T[] dbItems, T[] userItems, Func<T, T, bool> predicate, DbOperation op = DbOperation.All) where T : class {
      if (op.HasFlag(DbOperation.Delete)) {
        foreach (var itm in dbItems) {
          if (userItems.Any(a => predicate(a, itm)) == false)
            ent.Set<T>().Remove(itm);
        }
      }

      foreach (var itm in userItems) {
        var dbItm = dbItems.SingleOrDefault(a => predicate(a, itm));
        if (null != dbItm && op.HasFlag(DbOperation.Update))
          ent.Entry(dbItm).CurrentValues.SetValues(itm);
        else if (null == dbItm && op.HasFlag(DbOperation.Add))
          ent.Set<T>().Add(itm);
      }
    }
    #endregion
  }
}