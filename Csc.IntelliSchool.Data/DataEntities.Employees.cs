using System.Data.Entity;

namespace Csc.IntelliSchool.Data {
  public partial class DataEntities {
    public virtual DbSet<Employee> Employees { get; set; }
    public virtual DbSet<EmployeeSalary> EmployeeSalaries { get; set; }
    //public virtual DbSet<EmployeeSocialInsurance> EmployeeSocialInsurances { get; set; }
    public virtual DbSet<EmployeeBankAccount> EmployeeBankAccounts { get; set; }
    public virtual DbSet<EmployeeLoanInstallment> EmployeeLoanInstallments { get; set; }
    public virtual DbSet<EmployeeLoan> EmployeeLoans { get; set; }
    public virtual DbSet<EmployeeAttendance> EmployeeAttendance { get; set; }
    public virtual DbSet<EmployeeTerminalTransaction> EmployeeTerminalTransactions { get; set; }
    public virtual DbSet<EmployeeEarning> EmployeeEarnings { get; set; }
    public virtual DbSet<EmployeeCustody> EmployeeCustodies { get; set; }
    public virtual DbSet<EmployeeCustodyItem> EmployeeCustodyItems { get; set; }
    public virtual DbSet<EmployeeDocument> EmployeeDocuments { get; set; }
    public virtual DbSet<EmployeeSalaryUpdate> EmployeeSalaryUpdates { get; set; }
    public virtual DbSet<EmployeeDependant> EmployeeDependants { get; set; }
    public virtual DbSet<EmployeeAttendanceTimeOff> EmployeeAttendanceTimeOffs { get; set; }
    public virtual DbSet<EmployeeOfficialDocument> EmployeeOfficialDocuments { get; set; }
    public virtual DbSet<EmployeeOfficialDocumentType> EmployeeOfficialDocumentTypes { get; set; }
    public virtual DbSet<EmployeeDepartmentOfficialDocument> EmployeeDepartmentOfficialDocuments { get; set; }

    #region Structure
    public virtual DbSet<EmployeePosition> EmployeePositions { get; set; }
    public virtual DbSet<EmployeeTerminal> EmployeeTerminals { get; set; }
    public virtual DbSet<EmployeeBranch> EmployeeBranches { get; set; }
    public virtual DbSet<EmployeeDepartment> EmployeeDepartments { get; set; }
    public virtual DbSet<EmployeeTransactionRule> EmployeeTransactionRules { get; set; }
    public virtual DbSet<EmployeeList> EmployeeLists { get; set; }
    public virtual DbSet<EmployeePositionList> EmployeePositionLists { get; set; }
    public virtual DbSet<EmployeeDepartmentList> EmployeeDepartmentLists { get; set; }
    #endregion

    #region Vacations
    public virtual DbSet<EmployeeVacation> EmployeeVacations { get; set; }
    public virtual DbSet<EmployeeVacationType> EmployeeVacationTypes { get; set; }
    #endregion

    #region DepartmentVacations
    public virtual DbSet<EmployeeDepartmentVacation> EmployeeDepartmentVacations { get; set; }
    public virtual DbSet<EmployeeDepartmentVacationLink> EmployeeDepartmentVacationLinks { get; set; }
    #endregion

    #region Bonuses
    public virtual DbSet<EmployeeBonus> EmployeeBonuses { get; set; }
    public virtual DbSet<EmployeeBonusType> EmployeeBonusTypes { get; set; }
    #endregion

    #region Allowances
    public virtual DbSet<EmployeeAllowance> EmployeeAllowances { get; set; }
    public virtual DbSet<EmployeeAllowanceType> EmployeeAllowanceTypes { get; set; }
    #endregion

    #region Charges
    public virtual DbSet<EmployeeCharge> EmployeeCharges { get; set; }
    public virtual DbSet<EmployeeChargeType> EmployeeChargeTypes { get; set; }
    #endregion

    #region Deductions
    public virtual DbSet<EmployeeDeduction> EmployeeDeductions { get; set; }
    public virtual DbSet<EmployeeDeductionType> EmployeeDeductionTypes { get; set; }
    #endregion

    #region Medical
    public virtual DbSet<EmployeeMedicalConcession> EmployeeMedicalConcessions { get; set; }
    public virtual DbSet<EmployeeMedicalProgram> EmployeeMedicalPrograms { get; set; }
    public virtual DbSet<EmployeeMedicalRate> EmployeeMedicalRates { get; set; }
    public virtual DbSet<EmployeeMedicalCertificate> EmployeeMedicalCertificates { get; set; }
    public virtual DbSet<EmployeeMedicalProvider> EmployeeMedicalProviders { get; set; }
    public virtual DbSet<EmployeeMedicalClaimDocument> EmployeeMedicalClaimDocuments { get; set; }
    public virtual DbSet<EmployeeMedicalClaimType> EmployeeMedicalClaimTypes { get; set; }
    public virtual DbSet<EmployeeMedicalProgramTemplate> EmployeeMedicalProgramTemplates { get; set; }
    public virtual DbSet<EmployeeMedicalRequestDependant> EmployeeMedicalRequestDependants { get; set; }
    public virtual DbSet<EmployeeMedicalRequestEmployee> EmployeeMedicalRequestEmployees { get; set; }
    public virtual DbSet<EmployeeMedicalRequest> EmployeeMedicalRequests { get; set; }
    public virtual DbSet<EmployeeMedicalRequestType> EmployeeMedicalRequestTypes { get; set; }
    public virtual DbSet<EmployeeMedicalClaim> EmployeeMedicalClaims { get; set; }
    public virtual DbSet<EmployeeMedicalClaimStatus> EmployeeMedicalClaimStatuses { get; set; }
    public virtual DbSet<EmployeeMedicalProgramInfo> EmployeeMedicalProgramInfoes { get; set; }
    public virtual DbSet<EmployeeMedicalProposal> EmployeeMedicalProposals { get; set; }
    public virtual DbSet<EmployeeMedicalProposalDependant> EmployeeMedicalProposalDependants { get; set; }
    public virtual DbSet<EmployeeMedicalProposalEmployee> EmployeeMedicalProposalEmployees { get; set; }
    #endregion

    #region Shifts
    public virtual DbSet<EmployeeShift> EmployeeShifts { get; set; }
    public virtual DbSet<EmployeeShiftOverride> EmployeeShiftOverrides { get; set; }
    public virtual DbSet<EmployeeShiftOverrideType> EmployeeShiftOverrideTypes { get; set; }
    #endregion
  }

}