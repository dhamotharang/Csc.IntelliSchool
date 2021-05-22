using System;
using System.Threading;

namespace Csc.Components.Common {
  public class CountDownWatch {
    private int _count = 0;

    public Guid ID { get; protected set; }
    public event EventHandler Released;

    public CountDownWatch() {
      ID = Guid.NewGuid();
    }


    public static CountDownWatch Lock(int count = 1, EventHandler released =null) {
      var watch = new CountDownWatch();
      watch.Released += released;
      watch.Lock(count);
      return watch;
    }

    public void Lock(int count = 1) {
      if (count < 0)
        throw new ArgumentException();

        for (int i = 0; i < count; i++) {
          Interlocked.Increment(ref _count);
      }
    }

    public void Release(int count = 1) {
      if (count < 0)
        throw new ArgumentException();

      for (int i = 0; i < count; i++) {
        Interlocked.Decrement(ref _count);
      }

      if (_count <= 0)
        OnReleased();
    }

    public void Reset() {
      Interlocked.Exchange(ref _count, 0);
    }

    protected void OnReleased() {
      if (Released != null)
        Released(this, EventArgs.Empty);
    }
  }
}