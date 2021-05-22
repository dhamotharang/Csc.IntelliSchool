using Csc.Components.Common.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Csc.IntelliSchool.Data {
  public class EmployeeDynamicObject : DynObject {
    public static readonly string Key_ID = FormatColumnName("ID", DynObjectColumnFlags.Header  | DynObjectColumnFlags.Number);

    public static readonly string Key_DepartmentID = FormatColumnName("DepartmentID", DynObjectColumnFlags.Header | DynObjectColumnFlags.Hidden);
    public static readonly string Key_PositionID = FormatColumnName("PositionID", DynObjectColumnFlags.Header | DynObjectColumnFlags.Hidden);
    public static readonly string Key_BranchID = FormatColumnName("BranchID", DynObjectColumnFlags.Header | DynObjectColumnFlags.Hidden);
    public static readonly string Key_ListID = FormatColumnName("ListID", DynObjectColumnFlags.Header | DynObjectColumnFlags.Hidden);

    public static readonly string Key_FullName = FormatColumnName("Name", DynObjectColumnFlags.Header);

    public static readonly string Key_Gender = FormatColumnName("Gender", DynObjectColumnFlags.Header);
    public static readonly string Key_Marital = FormatColumnName("Marital", DynObjectColumnFlags.Header);
    public static readonly string Key_Nationality = FormatColumnName("Nationality", DynObjectColumnFlags.Header);

    public static readonly string Key_HireDate = FormatColumnName("Hire", DynObjectColumnFlags.Header | DynObjectColumnFlags.Date);
    public static readonly string Key_Department = FormatColumnName("Department", DynObjectColumnFlags.Header);
    public static readonly string Key_Position = FormatColumnName("Position", DynObjectColumnFlags.Header);
    public static readonly string Key_List = FormatColumnName("List", DynObjectColumnFlags.Header);


    [Display(AutoGenerateField = false)]
    public int EmployeeID { get { return this[Key_ID] != null ? (int)this[Key_ID] : 0; } set { this[Key_ID] = value; } }
    [Display(AutoGenerateField = false)]
    public int? DepartmentID { get { return this[Key_DepartmentID] as int?; } set { this[Key_DepartmentID] = value; } }
    [Display(AutoGenerateField = false)]
    public int? PositionID { get { return this[Key_PositionID] as int?; } set { this[Key_PositionID] = value; } }
    [Display(AutoGenerateField = false)]
    public int? BranchID { get { return this[Key_BranchID] as int?; } set { this[Key_BranchID] = value; } }
    [Display(AutoGenerateField = false)]
    public int? ListID { get { return this[Key_ListID] as int?; } set { this[Key_ListID] = value; } }


    [Display(AutoGenerateField = false)]
    public string FullName { get { return this[Key_FullName] as string; } set { this[Key_FullName] = value; } }

    [Display(AutoGenerateField = false)]
    public string Gender { get { return this[Key_Gender] as string; } set { this[Key_Gender] = value; } }
    [Display(AutoGenerateField = false)]
    public string Marital { get { return this[Key_Marital] as string; } set { this[Key_Marital] = value; } }
    [Display(AutoGenerateField = false)]
    public string Nationality { get { return this[Key_Nationality] as string; } set { this[Key_Nationality] = value; } }

    [Display(AutoGenerateField = false)]
    public DateTime? HireDate { get { return this[Key_HireDate] as DateTime?; } set { this[Key_HireDate] = value; } }
    [Display(AutoGenerateField = false)]
    public string Department { get { return this[Key_Department] as string; } set { this[Key_Department] = value; } }
    [Display(AutoGenerateField = false)]
    public string Position { get { return this[Key_Position] as string; } set { this[Key_Position] = value; } }
    [Display(AutoGenerateField = false)]
    public string List { get { return this[Key_List] as string; } set { this[Key_List] = value; } }

    public EmployeeDynamicObject() { }
    public EmployeeDynamicObject(Employee emp, EmployeeDynamicObjectAttributes attrib) {
        EmployeeID = emp.EmployeeID;
      FullName = emp.Person.FullName;

      if (attrib.HasFlag(EmployeeDynamicObjectAttributes.Personal)) {
        Gender = emp.Person.Gender;
        Marital = emp.Person.MaritalStatus;
        Nationality = emp.Person.Nationality?.Name;
      }

      if (attrib.HasFlag(EmployeeDynamicObjectAttributes.Employment)) {
        HireDate = emp.HireDate;
        Department = emp.Department != null ? emp.Department.Name : null;
        Position = emp.Position != null ? emp.Position.Name : null;
        List = emp.List != null ? emp.List.Name : null;
        DepartmentID = emp.DepartmentID;
        PositionID = emp.PositionID;
        BranchID = emp.BranchID;
        ListID = emp.ListID;
      }
    }
  }

  [Flags]
  public enum EmployeeDynamicObjectAttributes {
    Name = 0,
    Personal = 1 << 0,
    Employment = 1 << 1,
  }


}