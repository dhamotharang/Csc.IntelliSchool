
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("EmployeeOfficialDocumentTypes", Schema = "HR")]
  public partial class EmployeeOfficialDocumentType {

    public EmployeeOfficialDocumentType() {
      this.EmployeeDocuments = new HashSet<EmployeeOfficialDocument>();
      this.DepartmentDocuments = new HashSet<EmployeeDepartmentOfficialDocument>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TypeID { get; set; }
    public string Name { get; set; }
    public string Abbreviation { get; set; }
    public string Notes { get; set; }
    public Nullable<int> Order { get; set; }
    public bool IsMale { get; set; }
    public bool IsFemale { get; set; }
    public bool IsLocal { get; set; }
    public bool IsForeigner { get; set; }


    public virtual ICollection<EmployeeOfficialDocument> EmployeeDocuments { get; set; }

    public virtual ICollection<EmployeeDepartmentOfficialDocument> DepartmentDocuments { get; set; }
  }
}
