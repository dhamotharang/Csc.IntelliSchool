

using Csc.Components.Common.Data;
using Csc.Wpf.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EmployeeOfficialDocumentSummary : EmployeeDynamicObject {
    public EmployeeOfficialDocumentSummary() : base() { }
    public EmployeeOfficialDocumentSummary(Employee emp) : base(emp, EmployeeDynamicObjectAttributes.Personal | EmployeeDynamicObjectAttributes.Employment) {
    }

    public bool? GetDocument(string name) {

      return this[FormatColumnName(name, DynObjectColumnFlags.TrueFalse)] as bool?;
    }
    public void SetDocument(string name, bool? completed) {
      this[FormatColumnName(name, DynObjectColumnFlags.TrueFalse)] = completed;
    }
  }


}