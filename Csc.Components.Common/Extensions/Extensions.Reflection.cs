using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Reflection;

namespace Csc.Components.Common {
  public static partial class ReflectionExtensions {
    public static string GetMethodName(int frameIdx = 1) {
      StackTrace callStackTrace = new StackTrace();
      StackFrame propertyFrame = callStackTrace.GetFrame(frameIdx); // 1: below GetPropertyName frame
      return propertyFrame.GetMethod().Name;
    }

    public static string GetPropertyName() {
      return GetMethodName(2).Replace("get_", "").Replace("set_", "");
    }


    public static bool IsCollection(this PropertyInfo property) {
      return property.PropertyType != typeof(string) && property.PropertyType.GetInterface(typeof(IEnumerable<>).FullName) != null;
    }
  }
}
