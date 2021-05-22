
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace Csc.IntelliSchool.Data {


  [Table("EmployeeDepartmentOfficialDocuments", Schema = "HR")]
  public partial class EmployeeDepartmentOfficialDocument {
    [Key, Column(Order = 1)]
    [ForeignKey("Department")]
    public int DepartmentID { get; set; }
    public virtual EmployeeDepartment Department { get; set; }

    [Key, Column(Order = 2)]
    [ForeignKey("Type")]
    public int TypeID { get; set; }
    public virtual EmployeeOfficialDocumentType Type { get; set; }

    public Nullable<bool> IgnoreThis { get; set; }

  }
}
