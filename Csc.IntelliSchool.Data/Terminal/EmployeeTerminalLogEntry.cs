
using System; using System.ComponentModel.DataAnnotations.Schema; using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EmployeeTerminalLogEntry {
    public string IP { get; set; }
    public int UserID { get; set; }
    public DateTime DateTime { get; set; }
  }
}