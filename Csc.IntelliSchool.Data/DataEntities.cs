using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class DataEntities : DbContext {
    public static readonly string ConnectionString =
      "data source=.;initial catalog=Csc.IntelliSchool;persist security info=True;Integrated Security=SSPI;MultipleActiveResultSets=True;";


    public DataEntities() : base(ConnectionString) {
      System.Data.Entity.Database.SetInitializer<DataEntities>(null);
    }


    protected override void OnModelCreating(DbModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);
      modelBuilder.Entity<View>().HasOptional(e => e.ParentView).WithMany(e => e.ChildViews).HasForeignKey(m => m.ParentViewID);
    }

  }
}
