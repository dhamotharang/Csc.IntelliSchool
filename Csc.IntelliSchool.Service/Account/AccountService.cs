using Csc.IntelliSchool.Data;
using System;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class AccountService : DataService{
    private static object _lockObject = new object();
    private static AccountService _service;

    public static AccountService Instance {
      get {
        if (_service == null)
          lock (_lockObject)
            if (_service == null)
              _service = new AccountService();
        return _service;
      }
    }

    private AccountService() {

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