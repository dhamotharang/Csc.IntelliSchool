using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Csc.IntelliSchool.Sync {
  partial class Program {
    private static void SyncEmployees() {
      OldModel.OldEntities oldEnt = new OldModel.OldEntities();
      NewEntities newEnt = new NewModel.NewEntities();

      oldEnt.Configuration.AutoDetectChangesEnabled = false;
      oldEnt.Configuration.ProxyCreationEnabled = false;
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      newEnt.Configuration.ProxyCreationEnabled = false;

      var oldEmployees = oldEnt.employee_data.Include("department").Include("employee_types")
        .Where(s => s.RefID == null && s.department_id != 20).ToArray();
      var oldDependants = oldEnt.employeedependants.ToArray();
      var oldSalaries = oldEnt.salaries_monthly_facts.ToArray();
      var oldLoans = oldEnt.loans.ToArray();
      var oldDeductions = oldEnt.penalties.ToArray();
      var oldBonuses = oldEnt.bouns.ToArray();

      List<EmployeeDepartment> departments = new List<EmployeeDepartment>();
      List<EmployeePosition> positions = new List<EmployeePosition>();
      List<Nationality> nations = new List<Nationality>();

      foreach (var oldEmp in oldEmployees) {
        var newEmp = new NewModel.Employee();

        FillPersonalInfo(newEnt, nations, oldEmp, newEmp);
        FillEmploymentInfo(newEnt, departments, positions, oldEmp, newEmp);
        FillSalaryInfo(oldEmp, newEmp);
        FillDeductions(oldLoans, oldDeductions, oldBonuses, oldEmp, newEmp);
        FillEarnings(newEmp, oldEmp, oldSalaries);
        FillDependants(newEmp, oldDependants);

        newEnt.Employees.Add(newEmp);
      }

      newEnt.SaveChanges();
    }

    private static void FillDependants(Employee newEmp, OldModel.employeedependant[] oldDependants) {
      foreach (var oldDep in oldDependants.Where(s => s.EmployeeID == newEmp.LocalID)) {
        var newDep = new EmployeeDependant();
        newDep.FirstName = oldDep.Name;
        newDep.LastName = string.Empty;
        newDep.ArabicFirstName = string.Empty;
        newDep.ArabicLastName = string.Empty;


        newDep.Gender = oldDep.Gender;
        newDep.Birthdate = oldDep.Birthdate;
        newDep.NationalityID = newEmp.NationalityID;
        newDep.ReligionID = newEmp.ReligionID;
        newDep.Type = oldDep.Type;
        newEmp.Dependants.Add(newDep);

        if (string.IsNullOrWhiteSpace(oldDep.InsuranceProgram) == false) {
          newDep.EmployeeMedicalCertificate = new EmployeeMedicalCertificate() {
            Code = newEmp.LocalID.ToString(),
            ProgramID = oldDep.InsuranceProgram == "Silver" ? 2 : 3,
            EnrollmentDate = oldDep.InsuranceActiveDate ?? DateTime.Today,
            Rate = oldDep.InsuranceRate ?? 0,
            Concession = oldDep.InsuranceConcession ?? 0,
            Monthly = oldDep.InsuranceMonthly ?? 0,
            //IsDependant = true
          };
        }
      }
    }

    private static void FillEarnings(Employee newEmp, OldModel.employee_data oldEmp, OldModel.salaries_monthly_facts[] oldSalaries) {
      var empSalaries = oldSalaries.Where(s => s.employee_name == oldEmp.employee_name && s.Date >= newEmp.HireMonth).OrderBy(s => s.Date).ToArray();

      if (oldEmp.ex_employee == true) {
        var lastSal = empSalaries.LastOrDefault();
        if (lastSal != null) {
          newEmp.TerminationDate = lastSal.Date.AddMonths(1).AddDays(-1);
          if (lastSal.Date == new DateTime(2016, 12, 1))
            Debugger.Break();
        }
      }

      int prevSalary = 0;
      int prevHousing = 0;
      int prevSocial = 0;
      int prevTaxes = 0;
      int prevMedical = 0;

      foreach (var sal in empSalaries) {
        var earn = new EmployeeEarning() {
          Month = sal.Date,
          Salary = (int)sal.salary,
          Housing = (int)sal.housing,
          Social = (int)sal.insurance,
          Taxes = (int)sal.tax,
          Medical = sal.HealthInsurance ?? 0,

          AttendancePoints = (decimal)(sal.points ?? 0),
          AbsenceDays = (int)sal.absents,
          Loans = (int)sal.loans,
          DeductionValues = (int)((sal.pen + sal.pen_val) ?? 0),
          BonusValues = (int)((sal.bon + sal.bon_val) ?? 0),
        };

        newEmp.Earnings.Add(earn);

        if (earn.Salary != prevSalary || earn.Housing != prevHousing || earn.Social != prevSocial || earn.Taxes != prevTaxes || earn.Medical != prevMedical) {
          newEmp.SalaryUpdates.Add(new EmployeeSalaryUpdate() {
            Date = earn.Month,
            Salary = earn.Salary,
            Housing = earn.Housing,
            Social = earn.Social,
            Taxes = earn.Taxes,
            Medical = earn.Medical
          });

        }

        prevSalary = earn.Salary;
        prevHousing = earn.Housing;
        prevSocial = earn.Social;
        prevTaxes = earn.Taxes;
        prevMedical = earn.Medical;
      }



    }

    private static void FillDeductions(OldModel.loan[] oldLoans, OldModel.penalty[] oldDeductions, OldModel.boun[] oldBonuses, OldModel.employee_data oldEmp, Employee newEmp) {

      foreach (var oldLoan in oldLoans.Where(s => s.employee_id == oldEmp.employee_id)) {
        var newLoan = new EmployeeLoan();
        newLoan.Approved = true;
        newLoan.RequestDate = oldLoan.loan_date;
        newLoan.EmployeeLoanInstallments.Add(new EmployeeLoanInstallment() {
          Amount = oldLoan.loan_value,
          Month = new DateTime(oldLoan.loan_date.Year, oldLoan.loan_date.Month, 1),
        });
        newEmp.Loans.Add(newLoan);
      }

      foreach (var oldDed in oldDeductions.Where(s => s.employee_id == oldEmp.employee_id)) {
        newEmp.Deductions.Add(new EmployeeDeduction() {
          Date = oldDed.penalty_date,
          Points = oldDed.penalty_per_value,
          Value = oldDed.penalty_static_value,
          Notes = oldDed.penalty_reason,
          Approved = true
        });
      }
      foreach (var oldBon in oldBonuses.Where(s => s.employee_id == oldEmp.employee_id)) {
        newEmp.Bonuses.Add(new EmployeeBonus() {
          Date = oldBon.bouns_date,
          Points = oldBon.bouns_per_value,
          Value = oldBon.bouns_static_value,
          Notes = oldBon.bouns_reason,
          Approved = true
        });
      }
    }

    private static void FillSalaryInfo(OldModel.employee_data oldEmp, Employee newEmp) {
      newEmp.Salary = new EmployeeSalary();
      newEmp.Salary.Salary = oldEmp.salary;
      newEmp.Salary.Housing = oldEmp.housing;
      newEmp.Salary.Taxes = oldEmp.tax;
      newEmp.Salary.Social = oldEmp.insurance;
    }

    private static void FillEmploymentInfo(NewEntities newEnt, List<EmployeeDepartment> departments, List<EmployeePosition> positions, OldModel.employee_data oldEmp, Employee newEmp) {
      newEmp.HireDate = oldEmp.employee_working_date;
      if (oldEmp.department != null) {
        var itm = departments.SingleOrDefault(s => s.Name == oldEmp.department.department_name);
        if (itm == null) {
          itm = new EmployeeDepartment() { Name = oldEmp.department.department_name ?? string.Empty, ArabicName = "" };
          newEnt.EmployeeDepartments.Add(itm);
          departments.Add(itm);
        }
        newEmp.Department = itm;
      }
      if (oldEmp.employee_types != null) {
        var itm = positions.SingleOrDefault(s => s.Name == oldEmp.employee_types.type);
        if (itm == null) {
          itm = new EmployeePosition() { Name = oldEmp.employee_types.type ?? string.Empty, ArabicName = "" };
          newEnt.EmployeePositions.Add(itm);
          positions.Add(itm);
        }
        newEmp.Position = itm;
      }
      newEmp.Shift = newEnt.EmployeeShifts.SingleOrDefault(s => s.LocalID == oldEmp.shift_id);
      if (oldEmp.terminal_ip != null && oldEmp.terminal_ip.Length > 0) {
        newEmp.TerminalUserID = oldEmp.user_id;
        newEmp.Terminal = newEnt.EmployeeTerminals.SingleOrDefault(s => s.IP == oldEmp.terminal_ip);
      }
      if ((oldEmp.employee_add != null && oldEmp.employee_add.Length > 0) || (oldEmp.employee_phone != null && oldEmp.employee_phone.Length > 0)) {
        newEmp.Contact = new Contact();
        if (oldEmp.employee_add != null)
          newEmp.Contact.ContactAddresses.Add(new ContactAddress() { Address = oldEmp.employee_add, Reference = "Default", IsDefault = true });
        if (oldEmp.employee_phone != null)
          newEmp.Contact.ContactNumbers.Add(new ContactNumber() { Number = oldEmp.employee_phone, Reference = "Default", IsDefault = true });
      }

      if (oldEmp.ex_employee)
        newEmp.TerminationDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);


      FillMedicalInsurance(newEmp, oldEmp);
    }

    private static void FillMedicalInsurance(Employee newEmp, OldModel.employee_data oldEmp) {
      if (string.IsNullOrWhiteSpace(oldEmp.InsuranceProgram) == false) {
        newEmp.MedicalCertificate = new EmployeeMedicalCertificate() {
          Code = newEmp.LocalID.ToString(),
          ProgramID = oldEmp.InsuranceProgram == "Silver" ? 2 : 3,
          EnrollmentDate = oldEmp.InsuranceActiveDate ?? DateTime.Today,
          Rate = oldEmp.InsuranceRate ?? 0,
          Concession = oldEmp.InsuranceConcession ?? 0,
          Monthly = oldEmp.InsuranceMonthly ?? 0
        };
      }
    }

    private static void FillPersonalInfo(NewEntities newEnt, List<Nationality> nations, OldModel.employee_data oldEmp, Employee newEmp) {
      newEmp.EmployeeID = oldEmp.employee_id;
      newEmp.LocalID = oldEmp.employee_id;
      newEmp.FirstName = oldEmp.FirstName;
      newEmp.MiddleName = oldEmp.MiddleName;
      newEmp.LastName = oldEmp.LastName;
      newEmp.FamilyName = oldEmp.FamilyName;
      newEmp.ArabicFirstName = "";
      newEmp.ArabicLastName = "";
      newEmp.Gender = oldEmp.employee_gender ?? string.Empty;
      newEmp.Birthdate = oldEmp.employee_bd;
      newEmp.IDNumber = oldEmp.IDNumber;
      if (oldEmp.employee_nationality != null && oldEmp.employee_nationality.Trim().Length > 0) {
        var itm = nations.SingleOrDefault(s => s.Name.ToLower().Trim() == oldEmp.employee_nationality.ToLower().Trim());
        if (itm == null) {
          itm = new Nationality() { Name = oldEmp.employee_nationality ?? string.Empty, ArabicName = "" };
          newEnt.Nationalities.Add(itm);
          nations.Add(itm);
        }
        newEmp.Nationality = itm;
      }
    }
  }
}
