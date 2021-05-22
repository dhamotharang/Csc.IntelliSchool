
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace Csc.IntelliSchool.Data {

  [Table("Employees", Schema = "HR")]
  public partial class Employee {

    public Employee() {
      this.Loans = new HashSet<EmployeeLoan>();
      this.Deductions = new HashSet<EmployeeDeduction>();
      this.Vacations = new HashSet<EmployeeVacation>();
      this.Attendance = new HashSet<EmployeeAttendance>();
      this.Bonuses = new HashSet<EmployeeBonus>();
      this.Earnings = new HashSet<EmployeeEarning>();
      this.Custodies = new HashSet<EmployeeCustody>();
      this.CustodyItems = new HashSet<EmployeeCustodyItem>();
      this.Documents = new HashSet<EmployeeDocument>();
      this.SalaryUpdates = new HashSet<EmployeeSalaryUpdate>();
      this.Dependants = new HashSet<EmployeeDependant>();
      this.MedicalRequestEmployees = new HashSet<EmployeeMedicalRequestEmployee>();
      this.OfficialDocuments = new HashSet<EmployeeOfficialDocument>();
      this.EmployeeMedicalClaims = new HashSet<EmployeeMedicalClaim>();
      this.EmployeeMedicalProposalEmployees = new HashSet<EmployeeMedicalProposalEmployee>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int EmployeeID { get; set; }


    public Nullable<int> TerminalUserID { get; set; }
    public System.DateTime HireDate { get; set; }
    public Nullable<System.DateTime> TerminationDate { get; set; }
    public string TerminationReason { get; set; }
    public bool IsCustody { get; set; }
    public Nullable<bool> TerminationHide { get; set; }

    [ForeignKey("Person")]
    public Nullable<int> PersonID { get; set; }
    public virtual Person Person { get; set; }

    [ForeignKey("Branch")]
    public Nullable<int> BranchID { get; set; }
    public virtual EmployeeBranch Branch { get; set; }

    [ForeignKey("Department")]
    public Nullable<int> DepartmentID { get; set; }
    public virtual EmployeeDepartment Department { get; set; }

    [ForeignKey("Position")]
    public Nullable<int> PositionID { get; set; }
    public virtual EmployeePosition Position { get; set; }

    [ForeignKey("Shift")]
    public Nullable<int> ShiftID { get; set; }
    public virtual EmployeeShift Shift { get; set; }

    [ForeignKey("Terminal")]
    public Nullable<int> TerminalID { get; set; }
    public virtual EmployeeTerminal Terminal { get; set; }

    [ForeignKey("List")]
    public Nullable<int> ListID { get; set; }
    public virtual EmployeeList List { get; set; }

    public virtual EmployeeSalary Salary { get; set; }
    public virtual EmployeeSocialInsurance SocialInsurance { get; set; }


    public virtual ICollection<EmployeeLoan> Loans { get; set; }
    public virtual ICollection<EmployeeDeduction> Deductions { get; set; }
    public virtual ICollection<EmployeeVacation> Vacations { get; set; }
    public virtual ICollection<EmployeeAttendance> Attendance { get; set; }
    public virtual ICollection<EmployeeBonus> Bonuses { get; set; }
    public virtual ICollection<EmployeeEarning> Earnings { get; set; }

    public virtual ICollection<EmployeeCustody> Custodies { get; set; }

    public virtual ICollection<EmployeeCustodyItem> CustodyItems { get; set; }

    public virtual ICollection<EmployeeDocument> Documents { get; set; }

    public virtual ICollection<EmployeeSalaryUpdate> SalaryUpdates { get; set; }

    public virtual ICollection<EmployeeDependant> Dependants { get; set; }

    public virtual ICollection<EmployeeBankAccount> BankAccounts { get; set; }


    public virtual ICollection<EmployeeMedicalRequestEmployee> MedicalRequestEmployees { get; set; }

    public virtual ICollection<EmployeeOfficialDocument> OfficialDocuments { get; set; }

    public virtual ICollection<EmployeeMedicalClaim> EmployeeMedicalClaims { get; set; }

    public virtual ICollection<EmployeeMedicalProposalEmployee> EmployeeMedicalProposalEmployees { get; set; }

    #region Medical
    [ForeignKey("MedicalCertificate")]
    public Nullable<int> MedicalCertificateID { get; set; }
    public virtual EmployeeMedicalCertificate MedicalCertificate { get; set; }

    #endregion

  }
}
