using Csc.Components.Common;
using Csc.Components.Common.Data;
using Csc.Components.Data;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace Csc.IntelliSchool.Data {
  public static partial class DataModelExtensions {
    #region State
    public static void SetModified(this DbContext ctx, object entity) {
      SetState(ctx, entity, EntityState.Modified);
    }
    public static void SetUnchanged(this DbContext ctx, object entity) {
      if (entity == null)
        return;

      SetState(ctx, entity, EntityState.Unchanged);
    }
    public static void SetUnchanged(this DbContext ctx, params object[] entities) {

      if (entities == null)
        return;

      foreach (var obj in entities) {
        if (obj == null)
          return;

        SetState(ctx, obj, EntityState.Unchanged);
      }
    }
    public static void SetState(this DbContext ctx, object entity, EntityState state) {
      ctx.Entry(entity).State = state;
    }
    #endregion
  }
}