using Csc.IntelliSchool.Data;
using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;

namespace Csc.IntelliSchool.Service {
  public partial class GenericService : DataService {
    #region Ctr
    private static object _lockObject = new object();
    private static GenericService _service;

    public static GenericService Instance {
      get {
        if (_service == null)
          lock (_lockObject)
            if (_service == null)
              _service = new GenericService();
        return _service;
      }
    }

    private GenericService() {

    }
    #endregion

    #region Generic
    public T[] GetItems<T>() where T : class { return GetItems<T>(null, null); }
    public T[] GetItems<T>(Expression<Func<T, bool>> predicate) where T : class { return GetItems<T>(null, predicate); }
    public T[] GetItems<T>(string includePath) where T : class { return GetItems<T>(includePath, null); }
    public T[] GetItems<T>(string includePath, Expression<Func<T, bool>> predicate) where T : class {
      using (var ent = CreateModel()) {
        IQueryable<T> set = null;

        if (includePath != null)
          set = ent.Set<T>().Include(includePath).AsQueryable();
        else
          set = ent.Set<T>().AsQueryable();
        if (predicate != null)
          set = set.Where(predicate);
        return set.ToArray();
      }
    }

    public void DeleteItem<T>(params object[] keys) where T : class {
      using (var ent = CreateModel()) {
        var item = ent.Set<T>().Find(keys);
        if (null == item)
          return;

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Delete, item.GetType(), keys.Select(s => (int)s).ToArray());

        ent.Set<T>().Remove(item);
        ent.SaveChanges();
      }
    }
    public T AddOrUpdateItem<T>(T item) where T : class {
      using (var ent = CreateModel()) {
        var keys = ent.GetKeys(item);
        bool add = ent.Set<T>().Find(keys) == null;

        if (add)
          ent.Set<T>().Add(item);
        else {
          ent.Set<T>().Attach(item);
          ent.Entry(item).State = System.Data.Entity.EntityState.Modified;
          ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Update, item.GetType(), keys.Select(s => (int)s).ToArray());
        }
        ent.SaveChanges();

        if (add) {
          ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
          ent.Logger.Flush();
        }


        return item;
      }
    }
    public T AddItem<T>(T item) where T : class {
      using (var ent = CreateModel()) {
        ent.Set<T>().Add(item);

        ent.SaveChanges();

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
        ent.Logger.Flush();

        return item;
      }
    }
    public T UpdateItem<T>(T item) where T : class {
      using (var ent = CreateModel()) {
        ent.Set<T>().Attach(item);
        ent.Entry(item).State = System.Data.Entity.EntityState.Modified;

        ent.Logger.LogDatabase(CurrentUser, SystemLogDataAction.Update, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());

        ent.SaveChanges();

        return item;
      }
    }


    #endregion

    #region Child

    private void UpdateEntity<T>(DataEntities ent, T dbItem, T userItem) where T : class {
      ent.Entry(dbItem).CurrentValues.SetValues(userItem);
    }

    private void UpdateRelatedEntity<T>(DataEntities ent, T dbItem, T userItem) where T : class {
      if (userItem == null && dbItem != null)
        ent.Set<T>().Remove(dbItem);
      else if (userItem != null && dbItem != null)
        ent.Entry(dbItem).CurrentValues.SetValues(userItem);
      else if (userItem != null && dbItem == null)
        ent.Set<T>().Add(userItem);
    }
    private void UpdateChildEntities<T>(DataEntities ent, T[] dbItems, T[] userItems, Func<T, T, bool> predicate, DbOperation op = DbOperation.All) where T : class {
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

    #region Other
    protected long FindSize(object obj) {
      long size = 0;
      using (Stream s = new MemoryStream()) {
        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        formatter.Serialize(s, obj);
        size = s.Length;
      }
      return size;
    }
    #endregion

  }
}