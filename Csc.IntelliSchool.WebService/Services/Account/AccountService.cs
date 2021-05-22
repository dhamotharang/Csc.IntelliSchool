using Csc.IntelliSchool.Data;
using System;
using System.Linq;

namespace Csc.IntelliSchool.WebService.Services.Account {
  public partial class AccountService {
    public User Login(string username, string password) {
      using (var ent = ServiceManager.CreateModel()) {
        username = username.ToLower().Trim();

        var user = ent.GetUsersQuery().SingleOrDefault(s => s.Username != null && s.Username.Length > 0 && s.Username.ToLower() == username && s.IsLocked == false);
        if (null == user) {
          user = ValidateWindows(ent, username, password);
          if (null != user)
            user.LoginMode = LoginStatus.WindowsCredentials;
        } else {
          string passwordHash = password;
          if (user.PasswordFormat == PasswordFormat.Sha1.ToString())
            passwordHash = Components.Security.Cryptography.HashSha1(password);

          if (String.Compare(passwordHash, user.Password, false) == 0) {
            user.LoginMode = LoginStatus.System;
          } else {
            user = ValidateWindows(ent, username, password);
            if (null != user)
              user.LoginMode = LoginStatus.WindowsCredentials;
            else
              user = null;
          }
        }

        // TODO: Set as early initialization
        if (user != null) {
          var parentViews = user.UserViews.Select(s => s.View).Select(s => SelectParentView(s)).Where(s => s != null).Distinct().ToArray();
          foreach (var view in parentViews)
            SortViews(view);

          user.Views = parentViews;
        }

        if (user != null) {
          ent.Logger.LogSecurity(user, SystemLogSecurityAction.Login, new SystemLogSecurityEntry() { LoginMode = user.LoginMode.ToString() });
        } else {
          ent.Logger.LogSecurity(null, SystemLogSecurityAction.LoginFailed, new SystemLogSecurityEntry() { Username = username });
        }
        ent.Logger.Flush();


        return user;
      }
    }

    private static View SelectParentView(View view) {
      View parentView = null;

      if (view.ParentView == null)
        parentView = view;
      else
        parentView = SelectParentView(view.ParentView);

      return parentView;
    }

    private static void SortViews(View view) {
      if (view.ChildViews.Count() < 2)
        return;

      // views with child views > 0

      view.ChildViews =
        view.ChildViews.Where(s => s.ChildViews.Count > 0 && s.Order != null).OrderBy(s => s.Order).ThenBy(s => s.Name).ThenBy(s => s.Title)
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count > 0 && s.Order == null).OrderBy(s => s.Name).ThenBy(s => s.Title))
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count == 0 && s.Order != null).OrderBy(s => s.Order).ThenBy(s => s.Name).ThenBy(s => s.Title))
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count == 0 && s.Order == null).OrderBy(s => s.Name).ThenBy(s => s.Title))
        .ToArray();
      foreach (var childView in view.ChildViews)
        SortViews(childView);
    }

    private User ValidateWindows(DataEntities model, string username, string password) {
      var user = model.GetUsersQuery().SingleOrDefault(s => s.WindowsPrincipal.ToLower() == username && s.IsLocked == false);
      if (user == null)
        return null;

      if (Components.Security.PrincipalManagement.ValidateWindows(username, password))
        return user;

      return null;
    }

   
  }
}



//#region Update Password
//public  void UpdateUserPassword( int userId, string password) {
//  using (var model = CreateModel()) {
//    var user = model.Users.Single(s => s.UserID == userId);
//    user.Password = HashString(password);
//    user.PasswordFormat = PasswordFormat.Sha1.ToString();
//    model.SaveChanges();
//  }
//}
//#endregion


//#region Profile
//public  void UpdateUserProfile(int userId, string firstName, string lastName) {
//  using (var model = CreateModel()) {
//    var user = model.Users.Single(s => s.UserID == userId);
//    user.FirstName = firstName.Trim();
//    user.LastName = lastName.Trim();
//    model.SaveChanges();
//  }
//}
//#endregion

//#region Query
//public User[] GetUsers() {
//  User[] users = null;

//  using (var ent = CreateModel()) {
//    users = ent.GetUsersQuery().OrderBy(s => s.Username).ToArray();
//  }

//  return users.ToArray();
//}

//public  User[] GetUsers(int[] userIds) {
//  using (var ent = CreateModel()) {
//    var users = ent.GetUsersQuery().Where(s => userIds.Contains(s.UserID)).OrderBy(s => s.Username).ToArray();
//    return users.ToArray();
//  }
//}
//#endregion

//#region Update
//public  UpdateUserResult UpdateUser(User usr) {
//  if (string.IsNullOrWhiteSpace(usr.Username))
//    return new UpdateUserResult(UpdateUserResultCode.InvalidUsername);
//  if (string.IsNullOrEmpty(usr.Password) || usr.PasswordFormatTyped == PasswordFormat.Unknown)
//    return new UpdateUserResult(UpdateUserResultCode.InvalidPassword);

//  usr.Username = usr.Username.ToLower().Trim();

//  bool update = usr.UserID > 0;

//  using (var ent = CreateModel()) {
//    User dbUser = null;

//    if (update) { // Update
//      dbUser = ent.GetUsersQuery().SingleOrDefault(s => s.UserID == usr.UserID);
//      if (null == dbUser)
//        return new UpdateUserResult(UpdateUserResultCode.UserNotFound);
//    } else { // Create New
//      dbUser = usr;
//      ent.Users.Add(dbUser);
//    }

//    if (UserExists(usr, ent))
//      return new UpdateUserResult(UpdateUserResultCode.UsernameExists);

//    if (update) {
//      dbUser.Username = usr.Username;
//      dbUser.FirstName = usr.FirstName;
//      dbUser.LastName = usr.LastName;
//      if (usr.PasswordFormatTyped == PasswordFormat.Plain) {
//        dbUser.Password = usr.Password;
//        dbUser.PasswordFormatTyped = PasswordFormat.Plain;
//      }
//      dbUser.WindowsPrincipal = usr.WindowsPrincipal;
//      dbUser.CanChangePassword = usr.CanChangePassword;
//      dbUser.IsLocked = usr.IsLocked;
//    }

//    ent.SaveChanges();

//    return new UpdateUserResult(ent.GetUsersQuery().SingleOrDefault(s => s.UserID == usr.UserID));
//  }
//}

//protected bool UserExists(User usr, DataEntities ent) {
//  return ent.Users.Where(s => s.Username.ToLower() == usr.Username && s.UserID != usr.UserID).Count() > 0;
//}

//public  User DeleteUser(User usr) {
//  using (var ent = CreateModel()) {
//    var dbUser = ent.Users.SingleOrDefault(s => s.UserID == usr.UserID);
//    if (dbUser != null)
//      ent.Users.Remove(dbUser);
//    ent.SaveChanges();
//    return usr;
//  }
//}
//#endregion