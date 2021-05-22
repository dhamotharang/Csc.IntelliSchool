using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace Csc.Components.Common {
  public class JsonIgnoreReferenceResolver : DefaultContractResolver {
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
      JsonProperty prop = base.CreateProperty(member, memberSerialization);

      if (prop.PropertyType.IsClass &&
          prop.PropertyType != typeof(string)) {
        prop.ShouldSerialize = obj => false;
      }

      return prop;
    }
  }
}
