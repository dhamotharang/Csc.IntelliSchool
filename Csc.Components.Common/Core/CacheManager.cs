using System;
using System.Collections.Generic;
using System.Runtime.Caching;

namespace Csc.Components.Common {
  public class CacheManager {
    private List<string> _keys = new List<string>();

    public TimeSpan DefaultLifetime { get; set; }
    public string Name { get; private set; }
    public MemoryCache Cache { get; private set; }

    public CacheManager(string name) {
      this.Name = name;

      if (name == MemoryCache.Default.Name)
        this.Cache = MemoryCache.Default;
      else
        this.Cache = new MemoryCache(name);
      this.DefaultLifetime = TimeSpan.FromHours(1);
    }

    public void Add<T>(T itm, DateTimeOffset? absoluteExpiration = null) {
      Add(itm.GetType().FullName, itm, absoluteExpiration);
    }
    public void Add<T>(string key, T itm, DateTimeOffset? absoluteExpiration = null) {
      if (absoluteExpiration == null)
        absoluteExpiration = DateTime.Now.Add(DefaultLifetime);

        Remove(key);

      Cache.Add(key, itm, absoluteExpiration.Value);
      _keys.Add(key);
    }


    public T Get<T>() where T : class {
      return Get<T>(typeof(T).FullName);
    }
    public T Get<T>(string key) where T : class {
      return Get(key) as T;
    }
    public object Get(string key) {
      return Cache[key];
    }


    public bool Contains<T>()  {
      return Contains (typeof(T).FullName);
    }
    public bool Contains(string key) {
      return Cache.Contains(key);
    }

    public void Remove<T>() {
      Remove(typeof(T).FullName);
    }
    public void Remove(string key) {
      if (Contains(key)) {
        Cache.Remove(key);
        _keys.Remove(key);
      }
    }

    public void Clear() {
      foreach (var key in _keys.ToArray())
        Remove(key);
    }
  }

}
