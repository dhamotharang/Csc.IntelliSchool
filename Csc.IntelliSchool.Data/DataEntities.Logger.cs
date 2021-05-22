namespace Csc.IntelliSchool.Data {
  public partial class DataEntities {
    private object _lockObj = new object();
    private EntityLogger _logger;

    public EntityLogger Logger {
      get {
        if (_logger == null)
          lock (_lockObj)
            if (_logger == null)
              _logger = new EntityLogger(this);
        return _logger;
      }
    }

  }

}