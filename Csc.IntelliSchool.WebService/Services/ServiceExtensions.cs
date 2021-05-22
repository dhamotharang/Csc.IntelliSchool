using Csc.IntelliSchool.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace Csc.IntelliSchool.WebService.Services {
  public static class ServiceExtensions {
    public static string GetHeader(string key) {
      return WebOperationContext.Current.IncomingRequest.Headers[key];
    }


  }
}