using Csc.Components.Data;
using Csc.IntelliSchool.Data;

namespace Csc.IntelliSchool.Business {
  public static partial class AccountDataManager {
    // TODO: user view dependencies
    private static TableMonitorCollection UsersMonitorCollection { get; set; }

    private static void InitializeMonitorCollection() {
      var usrsDep = TableMonitor<Data.UserDependency>.Monitor(DataEntities.ConnectionString, null, TableChangeType.Update | TableChangeType.Delete);
      usrsDep.TableChanged += UserDependency_TableChanged;

      var usrsViewsDep = TableMonitor<Data.UserViewDependency>.Monitor(DataEntities.ConnectionString, null, TableChangeType.Delete);
      usrsViewsDep.TableChanged += UserViewsDependency_TableChanged;

      UsersMonitorCollection = new Components.Data.TableMonitorCollection();
      UsersMonitorCollection.Add(usrsDep);
      UsersMonitorCollection.Add(usrsViewsDep);
    }

    private static void BeginMonitorUserChanges() {
      EndMonitorUserChanges();
      UsersMonitorCollection.StartAllItems();
    }

    private static void EndMonitorUserChanges() {
      UsersMonitorCollection.StopAllItems();
    }


    private static void UserDependency_TableChanged(object sender, TableChangeEventArgs e) {
      var item = e.Entity as UserDependency;
      if (DataManager.CurrentUser != null && DataManager.CurrentUser.UserID == item.UserID) {
        RevalidateLogin();
      }
    }


    private static void UserViewsDependency_TableChanged(object sender, TableChangeEventArgs e) {
      var item = e.Entity as UserViewDependency;
      if (DataManager.CurrentUser != null && DataManager.CurrentUser.UserID == item.UserID) {
        Signout(null);
      }
    }
  }
}