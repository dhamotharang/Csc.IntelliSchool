using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Csc.Components.Common {
  public static partial class StringExtensions {
    public static string Combine(this IEnumerable<string> sources, string separator = " ", bool trim = true) {
      return string.Join(separator, sources.Where(s => s != null && (trim ? s.Trim().Length > 0 : true))
        .Select(s => trim ? s.Trim() : s));
    }

    public static void TrimStrings<T>(this T source) {
      foreach (var prop in source.GetType().GetProperties(System.Reflection.BindingFlags.Public)) {
        if (prop.PropertyType == typeof(string) && prop.CanWrite) {
          var value = prop.GetValue(source);
          if (value != null)
            prop.SetValue(source, value.ToString().Trim());
        }
      }
    }


    public static string ToTitleCase(this string str) {
      if (str == null)
        return null;

      return Regex.Replace(str, @"(^\w)|(\s\w)", m => m.Value.ToUpper());
      //return System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(str);
    }

    public static bool StartsWithArabicLetter(this string text) {
      foreach (var c in text) {
        if (char.IsLetter(c) == false)
          continue;

        return HasArabicLetter(c.ToString());
      }
      return false;
    }

    public static bool HasArabicLetter(this string text) {
      for (var i = 0; i < text.Length; i += char.IsSurrogatePair(text, i) ? 2 : 1) {
        var codepoint = char.ConvertToUtf32(text, i);
        if (Resources.Language.ArabicLeters.Contains(codepoint))
          return true;
      }
      return false;
    }
  }
}
