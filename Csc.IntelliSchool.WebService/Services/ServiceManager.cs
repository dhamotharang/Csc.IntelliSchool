using Csc.IntelliSchool.Data;
using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Csc.IntelliSchool.WebService.Services {
  internal static class ServiceManager {

    public static RemoteEndpointMessageProperty RemoteMessageNameProperty
    { get { return OperationContext.Current.IncomingMessageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty; } }


    public static DataEntities CreateModel(bool autoDetectChanges = true) {
      var ent = new DataEntities();
      ent.Configuration.AutoDetectChangesEnabled = autoDetectChanges;
      ent.Configuration.LazyLoadingEnabled = false;
      ent.Configuration.ProxyCreationEnabled = false;
      return ent;
    }


    public static string ClientIP
    {

      get
      {
        return RemoteMessageNameProperty.Address;
      }
    }
    //public static string ClientMachineName
    //{
    //  get
    //}
    public static int? CurrentUserID
    {
      get
      {
        var userIdStr = ServiceExtensions.GetHeader("UserID");
        if (userIdStr == null || userIdStr.Length == 0)
          return null;

        int userId = 0;

        if (int.TryParse (userIdStr, out userId))
          return userId;

        return null;
      }
    }

    public static User GetCurrentUser(DataEntities ent) {
      var userId = CurrentUserID;

      if (null == userId)
        return null;

      return ent.Users.SingleOrDefault(s => s.UserID == userId);
    }
  }
}