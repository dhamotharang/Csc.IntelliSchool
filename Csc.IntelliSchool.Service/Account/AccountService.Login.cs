using Csc.IntelliSchool.Data;
using System;
using System.Linq;

namespace Csc.IntelliSchool.Service {
  public partial class AccountService {
    public void Signout() {
      CurrentUser = null;
    }
    public User Login(string username, string password) {
        CurrentUser = null;

      using (var ent = CreateModel()) {
        username = username.ToLower().Trim();

        var user = ent.GetUsersQuery( UserDataFilter.Views).SingleOrDefault(s => s.Username != null && s.Username.Length > 0 && s.Username.ToLower() == username && s.IsLocked == false);
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

        CurrentUser = user;
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
        view.ChildViews.Where(s => s.ChildViews.Count > 0 && s.Order != null).OrderBy(s => s.Order).ThenBy(s => s.Name)
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count > 0 && s.Order == null).OrderBy(s => s.Name))
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count == 0 && s.Order != null).OrderBy(s => s.Order).ThenBy(s => s.Name))
        .Concat(view.ChildViews.Where(s => s.ChildViews.Count == 0 && s.Order == null).OrderBy(s => s.Name))
        .ToArray();
      foreach (var childView in view.ChildViews)
        SortViews(childView);
    }

    private User ValidateWindows(DataEntities model, string username, string password) {
      var user = model.GetUsersQuery(UserDataFilter.Views).SingleOrDefault(s => s.WindowsPrincipal.ToLower() == username && s.IsLocked == false);
      if (user == null)
        return null;

      if (Components.Security.PrincipalManagement.ValidateWindows(username, password))
        return user;

      return null;
    }

   
  }
}


