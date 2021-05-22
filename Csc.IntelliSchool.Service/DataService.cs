using Csc.IntelliSchool.Data;
using System.Transactions;

namespace Csc.IntelliSchool.Service {
  public abstract class DataService {
    protected static User CurrentUser { get;  set; }

    protected static DataEntities CreateModel(bool autoDetectChanges = true) {
      var ent = new DataEntities();
      ent.Configuration.AutoDetectChangesEnabled = autoDetectChanges;
      ent.Configuration.LazyLoadingEnabled = false;
      ent.Configuration.ProxyCreationEnabled = false;
      return ent;
    }

    protected static TransactionScope CreateTransaction() {
      return new TransactionScope();
    }
  }
}