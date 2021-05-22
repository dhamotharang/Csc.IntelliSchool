using System;

namespace Csc.Components.Data {
  [System.AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = true)]
  public sealed class DataIncludeAttribute : Attribute {
    public string[] IncludePaths { get; set; }

    public DataIncludeAttribute(params string[] includePaths) {
      this.IncludePaths = includePaths;
    }
  }
}
