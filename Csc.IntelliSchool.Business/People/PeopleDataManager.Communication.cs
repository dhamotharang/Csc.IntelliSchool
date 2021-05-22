
using Csc.Components.Common;
namespace Csc.IntelliSchool.Business {
  public static partial class PeopleDataManager {
    private static string[] ContactReferences {
      get { return DataManager.Cache.Get<string[]>(); }
      set {
        if (value == null)
          DataManager.Cache.Remove<string[]>();
        else
          DataManager.Cache.Add(value);
      }
    }
    public static string ContactDefaultReference { get { return ContactReferences != null && ContactReferences.Length > 0 ? ContactReferences[0] : null; } }

    public static void GetContactReferences(bool forceLoad, AsyncState<string[]> callback) {
      if (forceLoad || ContactReferences == null) {
        Async.AsyncCall(() => Service.PeopleService.Instance.GetContactReferences(), (res, err) => {
          if (err == null)
            ContactReferences = res;
          Async.OnCallback(res, err, callback);
        });
      } else
        Async.OnCallback(ContactReferences, null, callback);
    }
  }
}