using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace Csc.Components.Common {
  public static partial class CollectionExtensions {
    public static object ArrayGet(IEnumerable<object> items, int idx) { return items != null ? items.ElementAtOrDefault(idx) : null; }

    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source, IEqualityComparer<T> comparer = null) {
      return new HashSet<T>(source, comparer);
    }

    #region Array Packing
    public static T[][] PackArray<T>(this T[] array) {
      return new T[][] { array };
    }

    public static T[] PackArray<T>(this T item) {
      if (item == null)
        return null;

      return new T[] { item };
    }

    public static object[] PackObjectArray<T>(this T item) {
      if (item == null)
        return null;

      return new object[] { item };
    }
    #endregion

    #region Hierarchy
    public static T[] SelectHierarchy<T>(this IEnumerable<T> items, Func<T, IEnumerable<T>> selector) {
      List<T> resultItems = new List<T>();
      foreach (var item in items) {
        resultItems.Add(item);
        resultItems.AddRange(InternalSelectHierarchy(item, selector));
      }

      return resultItems.ToArray();
    }
    public static T[] SelectHierarchy<T>(this T item, Func<T, IEnumerable<T>> selector) {
      List<T> resultItems = new List<T>();
      resultItems.Add(item);
      resultItems.AddRange(InternalSelectHierarchy(item, selector));
      return resultItems.ToArray();
    }

    private static IEnumerable<T> InternalSelectHierarchy<T>(T parent, Func<T, IEnumerable<T>> selector) {
      List<T> items = new List<T>();
      foreach (var itm in selector(parent)) {
        items.Add(itm);
        items.AddRange(InternalSelectHierarchy(itm, selector));
      }

      return items.ToArray();
    }
    #endregion




    #region Contains
    public static bool ContainsAny(this string haystack, params string[] needles) {
      return ContainsItems(haystack, needles).Count() > 0;
    }

    public static IEnumerable<string> ContainsItems(this string haystack, IEnumerable<string> needles) {
      foreach (string x in needles) {
        if (haystack.Contains(x))
          yield return x;
      }

      yield break;
    }

    #endregion

  }



}
