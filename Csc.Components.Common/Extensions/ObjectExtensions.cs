using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Csc.Components.Common {
  public static partial class ObjectExtensions {
    public static T Clone<T>(this T source, bool includeReferences = true) {
      if (Object.ReferenceEquals(source, null)) {
        return default(T);
      }

      var settings = new JsonSerializerSettings {
        ObjectCreationHandling = ObjectCreationHandling.Replace,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
        PreserveReferencesHandling = PreserveReferencesHandling.Objects
      };
      if (includeReferences == false)
        settings.ContractResolver = new JsonIgnoreReferenceResolver();

      string serialized = JsonConvert.SerializeObject(source, settings);

      return JsonConvert.DeserializeObject<T>(serialized, settings);
    }

  }
}
