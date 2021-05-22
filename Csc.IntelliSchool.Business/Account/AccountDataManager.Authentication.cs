using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System;
using System.Linq;

namespace Csc.IntelliSchool.Business {
  public static partial class AccountDataManager {
    public static event EventHandler SignedOut;
    private static string _username, _password;

    #region Authentication
    // TODO: Reapply changes to CurrentUser

    public static void Login(string username, string password, AsyncState<bool> callback) {
      DataManager.CurrentUser = null;
      _username = username;
      _password = password;
      EndMonitorUserChanges();

      Async.AsyncCall(() => Service.AccountService.Instance.Login(username, password), (res, err) => {
        DataManager.CurrentUser = res;

        if (res != null) 
          BeginMonitorUserChanges();

        Async.OnCallback(res != null, err, callback);
      });
    }


    public static void RevalidateLogin() {
      // TODO: Update current views
      Login(_username, _password, (res, err) => {
        if (res == false)
          Signout(null);
      });
    }

    public static void Signout(AsyncState callback) {
      DataManager.Cache.Clear();
      DataManager.CurrentUser = null;
      _username = _password = null;
      EndMonitorUserChanges();

      Async.AsyncCall(Service.AccountService.Instance.Signout, callback);

      if (SignedOut != null)
        SignedOut(null, EventArgs.Empty);
    }

    #endregion
  }
}



//public static void UpdateUserPassword(string password, AsyncState callback) { Async.AsyncCall(() => Handler.UpdateUserPassword(CurrentUser.UserID, password), callback); }
//public static void UpdateUserProfile(string firstName, string lastName, AsyncState callback) { Async.AsyncCall(() => Handler.UpdateUserProfile(CurrentUser.UserID, firstName, lastName), callback); }

//public static void GetUsers(bool excludeCurrent, AsyncState<User[]> callback) { Async.AsyncCall(() => Handler.GetUsers(), callback); }
//public static void GetUsers(int[] userIds, AsyncState<User[]> callback) { Async.AsyncCall(() => Handler.GetUsers(userIds), callback); }
//public static void UpdateUser(User usr, AsyncState<UpdateUserResult> callback) { Async.AsyncCall(() => Handler.UpdateUser(usr), callback); }
//public static void DeleteUser(User usr, AsyncState<User> callback) { Async.AsyncCall(() => Handler.DeleteUser(usr), callback); }