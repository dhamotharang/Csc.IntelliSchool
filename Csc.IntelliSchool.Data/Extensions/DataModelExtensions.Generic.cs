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
    #region Generic
    public static T[] GetItems<T>(this DbContext ent) where T : class { return GetItems<T>(ent, null, null); }
    public static T[] GetItems<T>(this DbContext ent, Expression<Func<T, bool>> predicate) where T : class { return GetItems<T>(ent, null, predicate); }
    public static T[] GetItems<T>(this DbContext ent, string includePath) where T : class { return GetItems<T>(ent, includePath, null); }
    public static T[] GetItems<T>(this DbContext ent, string includePath, Expression<Func<T, bool>> predicate) where T : class {
      IQueryable<T> set = null;

      if (includePath != null)
        set = ent.Set<T>().Include(includePath).AsQueryable();
      else
        set = ent.Set<T>().AsQueryable();
      if (predicate != null)
        set = set.Where(predicate);
      return set.ToArray();
    }

    public static T RemoveItem<T>(this DbContext ent, int key) where T : class {
      return RemoveItem<T>(ent, key.PackObjectArray());
    }
    public static T RemoveItem<T>(this DbContext ent, params object[] keys) where T : class {
      var item = ent.Set<T>().Find(keys);
      if (null == item)
        return null;

      ent.Set<T>().Remove(item);

      ent.SaveChanges();

      return item;
    }
    public static T AddOrUpdateItem<T>(this DbContext ent, T item) where T : class {
      var keys = GetKeys(ent, item);
      var dbItem = ent.Set<T>().Find(keys);

      if (dbItem == null)
        ent.Set<T>().Add(item);
      else {
        ent.Entry(dbItem).State = EntityState.Detached;


        ent.Set<T>().Attach(item);
        ent.Entry(item).State = EntityState.Modified;
      }

      ent.SaveChanges();

      return item;
    }
    public static T AddItem<T>(this DbContext ent, T item) where T : class {
      ent.Set<T>().Add(item);

      ent.SaveChanges();

      return item;
    }
    public static T UpdateItem<T>(this DbContext ent, T item) where T : class {
      ent.Set<T>().Attach(item);
      ent.Entry(item).State = EntityState.Modified;

      ent.SaveChanges();

      return item;
    }
    #endregion
  }
}

//#region Generic
//public static T[] GetItems<T>() where T : class { return GetItems<T>(null, null); }
//public static T[] GetItems<T>(Expression<Func<T, bool>> predicate) where T : class { return GetItems<T>(null, predicate); }
//public static T[] GetItems<T>(string includePath) where T : class { return GetItems<T>(includePath, null); }
//public static T[] GetItems<T>(string includePath, Expression<Func<T, bool>> predicate) where T : class {
//  using (var ent = CreateModel()) {
//    IQueryable<T> set = null;

//    if (includePath != null)
//      set = ent.Set<T>().Include(includePath).AsQueryable();
//    else
//      set = ent.Set<T>().AsQueryable();
//    if (predicate != null)
//      set = set.Where(predicate);
//    return set.ToArray();
//  }
//}

//public static void DeleteItem<T>(params object[] keys) where T : class {
//  using (var ent = CreateModel()) {
//    var item = ent.Set<T>().Find(keys);
//    if (null == item)
//      return;

//    ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Delete, item.GetType(), keys.Select(s => (int)s).ToArray());

//    ent.Set<T>().Remove(item);
//    ent.SaveChanges();
//  }
//}
//public static T AddOrUpdateItem<T>(T item) where T : class {
//  using (var ent = CreateModel()) {
//    var keys = ent.GetKeys(item);
//    bool add = ent.Set<T>().Find(keys) == null;

//    if (add)
//      ent.Set<T>().Add(item);
//    else {
//      ent.Set<T>().Attach(item);
//      ent.Entry(item).State = EntityState.Modified;
//      ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Update, item.GetType(), keys.Select(s => (int)s).ToArray());
//    }
//    ent.SaveChanges();

//    if (add) {
//      ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
//      ent.Logger.Flush();
//    }


//    return item;
//  }
//}
//public static T AddItem<T>(T item) where T : class {
//  using (var ent = CreateModel()) {
//    ent.Set<T>().Add(item);

//    ent.SaveChanges();

//    ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
//    ent.Logger.Flush();

//    return item;
//  }
//}
//public static T UpdateItem<T>(T item) where T : class {
//  using (var ent = CreateModel()) {
//    ent.Set<T>().Attach(item);
//    ent.Entry(item).State = EntityState.Modified;

//    ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Update, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());

//    ent.SaveChanges();

//    return item;
//  }
//}
//#endregion