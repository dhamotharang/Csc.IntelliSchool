using System.Data.Entity;

namespace Csc.IntelliSchool.Data {
  public partial class DataEntities {

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<View> Views { get; set; }
    public virtual DbSet<UserView> UserViews { get; set; }
 
    public virtual DbSet<Bank> Banks { get; set; }


    public virtual DbSet<SystemFlag> SystemFlags { get; set; }
 
    public virtual DbSet<ContactReference> ContactReferences { get; set; }
  
    public virtual DbSet<SystemLog> SystemLogs { get; set; }
    public virtual DbSet<SystemLogData> SystemLogData { get; set; }



  }

}