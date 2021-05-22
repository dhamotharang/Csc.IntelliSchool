using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace Csc.Components.Common {
  public static class Async {
    public static Dispatcher Dispatcher { get; set; }

    public static void AsyncCall(Action task, AsyncState callback) {
      Task.Run(task).ContinueWith(t => { if (callback != null)  callback(t.Exception != null ? t.Exception.InnerException : null); }, TaskScheduler.FromCurrentSynchronizationContext());
    }
    public static void AsyncCall<T>(Func<T> task, AsyncState<T> callback) {
      Task<T>.Run(task)
        .ContinueWith(t => {
          if (callback != null) {
            Exception ex = t.Exception;
            if (ex != null && ex.InnerException != null)
              ex = ex.InnerException;
            if (ex != null)
              callback(default(T), ex);
            else
              callback(t.Result, null);
          }
        }, TaskScheduler.FromCurrentSynchronizationContext());
    }

    public static void OnCallback(Exception err, AsyncState callback) {
      if (callback != null)
        callback(err);
    }
    public static void OnCallback<T>(T result, Exception err, AsyncState<T> callback) {
      if (callback != null)
        callback(result, err);
    }
    public static void OnSingleCallback<T>(T[] result, Exception err, AsyncState<T> callback) {
      if (callback != null)
        callback(result != null ? result.FirstOrDefault() : default(T), err);
    }

    public static void BeginInvoke(Action invoke) {
      if (Dispatcher != null)
        Dispatcher.BeginInvoke(invoke);
      else
        invoke();
    }

  }

  public delegate void AsyncState(Exception error);
  public delegate void AsyncState<T>(T result, Exception error);
}
