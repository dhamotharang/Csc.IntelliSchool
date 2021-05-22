using System;
using System.Linq;
using Csc.Components.Common;
using System.Linq.Expressions;
using Csc.IntelliSchool.Data;
using System.Data.Entity;

namespace Csc.IntelliSchool.WebService.Services.Common {
  public partial class CommonDataService {
    //#region Generic
    //public void DeleteItem(Type type, params object[] keys) {
    //  using (var ent = ServiceManager.CreateModel()) {
    //    var set = ent.Set(type);

    //    var item = set.Find(keys);
    //    if (null == item)
    //      return;

    //    ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Delete, item.GetType(), keys.Select(s => (int)s).ToArray());

    //    set.Remove(item);
    //    ent.SaveChanges();
    //  }
    //}
    //public object AddOrUpdateItem(object item) {
    //  using (var ent = ServiceManager.CreateModel()) {
    //    Type type = item.GetType();

    //    var set = ent.Set(type);
    //    var keys = ent.GetKeys(item);
    //    bool add = set.Find(keys) == null;

    //    if (add)
    //      set.Add(item);
    //    else {
    //      set.Attach(item);
    //      ent.Entry(item).State = EntityState.Modified;
    //      ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Update, item.GetType(), keys.Select(s => (int)s).ToArray());
    //    }
    //    ent.SaveChanges();

    //    if (add) {
    //      ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
    //      ent.Logger.Flush();
    //    }

    //    return item;
    //  }
    //}
    //public object AddItem(object item) {
    //  using (var ent = ServiceManager.CreateModel()) {
    //    ent.Set(item.GetType()).Add(item);

    //    ent.SaveChanges();

    //    ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Insert, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());
    //    ent.Logger.Flush();

    //    return item;
    //  }
    //}
    //public object UpdateItem(object item) {
    //  using (var ent = ServiceManager.CreateModel()) {
    //    ent.Set(item.GetType()).Attach(item);
    //    ent.Entry(item).State = EntityState.Modified;

    //    ent.Logger.LogDatabase(ServiceManager.GetCurrentUser(ent), SystemLogDataAction.Update, item.GetType(), ent.GetKeys(item).Select(s => (int)s).ToArray());

    //    ent.SaveChanges();

    //    return item;
    //  }
    //}
    //#endregion
  }
}

