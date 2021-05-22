using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;
using Csc.Components.Common;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    #region Shifts
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeShift[] GetShifts();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeShift AddOrUpdateShift(EmployeeShift item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteShift(int id);
    #endregion


    #region ShiftOverrides
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeShiftOverride[] GetShiftOverrides();
    [OperationContract][ReferencePreservingDataContractFormat]
    EmployeeShiftOverride AddOrUpdateShiftOverride(EmployeeShiftOverride item);
    [OperationContract][ReferencePreservingDataContractFormat]
    void DeleteShiftOverride(int id);
    #endregion
  }

}