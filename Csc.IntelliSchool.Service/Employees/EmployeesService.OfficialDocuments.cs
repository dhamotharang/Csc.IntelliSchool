using System;
using System.Linq;
using Csc.Components.Common;
using Csc.IntelliSchool.Data;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Service {
  public partial class EmployeesService {
    public EmployeeOfficialDocument[] GetEmployeeOfficialDocuments(Employee emp) {
      if (emp.DepartmentID == 0)
        return new EmployeeOfficialDocument[] { };

      using (var ent = CreateModel()) {
        bool isMale = emp.Person.GenderTyped == Gender.Male;
        bool isFemale = emp.Person.GenderTyped == Gender.Female;

        var documentTypes = ent.EmployeeDepartmentOfficialDocuments.Include(EmployeeDepartmentOfficialDocumentIncludes.Type)
          .Where(s => s.DepartmentID == emp.DepartmentID.Value &&
            (s.Type.IsMale == isMale || s.Type.IsFemale == isFemale)).ToArray();


        if (emp.Person.Nationality != null)
          documentTypes = documentTypes.Where(s => emp.Person.Nationality.IsLocal == s.Type.IsLocal || !(emp.Person.Nationality.IsLocal == s.Type.IsForeigner)).ToArray();

        var documentTypeId = documentTypes.Select(s => s.TypeID).ToArray();
        var employeeDocuments = ent.EmployeeOfficialDocuments.Include(EmployeeDepartmentOfficialDocumentIncludes.Type).Where(s => s.EmployeeID == emp.EmployeeID && documentTypeId.Contains(s.TypeID)).ToList();
        var employeeDocumentTypeIds = employeeDocuments.Select(s => s.TypeID).ToArray();

        foreach (var type in documentTypes.Where(s => employeeDocumentTypeIds.Contains(s.TypeID) == false).ToArray()) {
          var doc = new EmployeeOfficialDocument() {
            Type = type.Type,
            TypeID = type.TypeID,
            EmployeeID = emp.EmployeeID,
            IsCompleted = false
          };

          employeeDocuments.Add(doc);
        }

        return employeeDocuments.OrderBy(s => s.Type.Order).ThenBy(s => s.Type.Name).ToArray();
      }
    }

    public EmployeeOfficialDocumentType[] GetEmployeeOfficialDocumentTypes(Employee emp) {
      if (emp.DepartmentID == 0)
        return new EmployeeOfficialDocumentType[] { };

      using (var ent = CreateModel()) {
        bool isMale = emp.Person.GenderTyped == Gender.Male;
        bool isFemale = emp.Person.GenderTyped == Gender.Female;

        var results = ent.EmployeeDepartmentOfficialDocuments.Include(EmployeeDepartmentOfficialDocumentIncludes.Type)
          .Where(s => s.DepartmentID == emp.DepartmentID.Value &&
            (s.Type.IsMale == isMale || s.Type.IsFemale == isFemale) &&
            (emp.Person.Nationality == null || (emp.Person.Nationality.IsLocal == s.Type.IsLocal || !emp.Person.Nationality.IsLocal == s.Type.IsForeigner))).Select(s => s.Type).Distinct().ToArray();


        return results;
      }
    }


    public EmployeeOfficialDocumentSummary[] GetEmployeeOfficialDocumentSummary(DateTime month, int[] listIds, int[] employeeIds) {
      List<EmployeeOfficialDocumentSummary> result = new List<EmployeeOfficialDocumentSummary>();
      month = month.ToMonth();

      using (var ent = CreateModel()) {
        var employees = ent.Employees.Query(EmployeeIncludes.Employment | EmployeeIncludes.Personal | EmployeeIncludes.OfficialDocuments, 
          new EmployeeDataCriteria() {
            StartDate = month.ToMonth(),
            EndDate = month.ToMonthEnd(),
            ListIDs = listIds, 
            EmployeeIDs = employeeIds
          }).ToArray();
        var departmentIds = employees.Where(s => s.DepartmentID != null).Select(s => s.DepartmentID.Value).Distinct().ToArray();


        var allDepartmentDocs = ent.EmployeeDepartmentOfficialDocuments.Include(EmployeeDepartmentOfficialDocumentIncludes.Type)
          .Where(s => departmentIds.Contains(s.DepartmentID))
          .OrderBy(s => s.Type.Order).ThenBy(s => s.Type.Name).ToArray();
        var allDocTypes = allDepartmentDocs.Select(s => s.Type).Distinct().ToArray();

        foreach (var dept in employees.GroupBy(s => s.DepartmentID).ToArray()) {
          var deptDocs = allDepartmentDocs.Where(s => s.DepartmentID == (dept.Key ?? 0)).Select(s => s.Type).ToArray();

          foreach (var emp in dept) {
            bool isMale = emp.Person.GenderTyped == Gender.Male;
            bool isFemale = emp.Person.GenderTyped == Gender.Female;

            var sum = new EmployeeOfficialDocumentSummary(emp);

            foreach (var type in allDocTypes) {
              if (deptDocs.Where(s => s.TypeID == type.TypeID &&
                (s.IsMale == isMale || s.IsFemale == isFemale) &&
                (emp.Person.Nationality == null || (emp.Person.Nationality.IsLocal == s.IsLocal || !emp.Person.Nationality.IsLocal == s.IsForeigner))).Count() > 0) {
                var empDoc = emp.OfficialDocuments.SingleOrDefault(s => s.TypeID == type.TypeID);

                sum.SetDocument(type.DisplayName, empDoc != null && empDoc.IsCompleted == true);
              } else // not required
                sum.SetDocument(type.DisplayName, null);
            }

            result.Add(sum);
          }
        }
      }

      return result.OrderBy(s => s.FullName).ToArray();
    }

  }

}
