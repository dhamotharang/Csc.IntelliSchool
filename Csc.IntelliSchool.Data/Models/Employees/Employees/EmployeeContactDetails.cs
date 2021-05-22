

using Csc.Components.Common.Data;
using Csc.Wpf.Data;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EmployeeContactDetails : EmployeeDynamicObject {
    public EmployeeContactDetails() : base() { }
    public EmployeeContactDetails(Employee emp) : base(emp,  EmployeeDynamicObjectAttributes.Employment) {
    }

    public string GetContactNumber(string referecnce) {

      return this[FormatColumnName("Num " + referecnce, DynObjectColumnFlags.Text)] as string;
    }
    public void SetContactNumber(int order, string number, string reference) {
      this[FormatColumnName("Num " + order.ToString(), DynObjectColumnFlags.Text)] = number;
      this[FormatColumnName("Num " + order.ToString() + " Ref", DynObjectColumnFlags.Text)] = reference;
    }

    public string GetContactAddress(string referecnce) {

      return this[FormatColumnName("Address " + referecnce, DynObjectColumnFlags.Text)] as string;
    }
    public void SetContactAddress(string referecnce, string value) {
      this[FormatColumnName("Address " + referecnce, DynObjectColumnFlags.Text)] = value;
    }
  }


}