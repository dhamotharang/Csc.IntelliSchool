using System.Collections.Generic;
using System.Linq;
namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    public static IQueryable<User> GetUsersQuery(this DataEntities model, UserDataFilter filter = UserDataFilter.None) {
      return model.Users
        .Include(GetUserIncludes(filter))


        .AsQueryable();
    }

    public static string[] GetUserIncludes(UserDataFilter filter) {
      List<string> includes = new List<string>();

      if (filter.HasFlag(UserDataFilter.UserViews))
        includes.AddRange(new string[] { "UserViews" });
      else if (filter.HasFlag(UserDataFilter.Views))
        includes.AddRange(new string[] {
          "UserViews.View.ParentView.ParentView.ParentView.ParentView.ParentView.ParentView.ParentView.ParentView.ParentView.ParentView",
          "UserViews.View.Group"
        });


      return includes.Distinct().ToArray();
    }

  }
}