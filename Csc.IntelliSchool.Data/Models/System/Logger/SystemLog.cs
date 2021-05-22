
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("SystemLogs", Schema = "App")]
  public partial class SystemLog {

    public SystemLog() {
      this.Data = new HashSet<SystemLogData>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int LogID { get; set; }
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    public Nullable<System.DateTime> DateTimeUtc { get; set; }
    public Nullable<int> UserID { get; set; }
    public string Username { get; set; }
    public string Category { get; set; }
    public string Action { get; set; }
    public string Description { get; set; }
    public string Computer { get; set; }
    public string Level { get; set; }
    public string AppVersion { get; set; }
    public string Table { get; set; }
    public string References { get; set; }


    public virtual ICollection<SystemLogData> Data { get; set; }
  }
}
