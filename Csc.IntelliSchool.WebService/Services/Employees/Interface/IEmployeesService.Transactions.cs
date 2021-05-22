using Csc.IntelliSchool.Data;
using System;
using System.ServiceModel;

namespace Csc.IntelliSchool.WebService.Services.HumanResources {
  public partial interface IEmployeesService {
    [OperationContract][ReferencePreservingDataContractFormat]
    
    EmployeeTerminalTransaction[] GetEmployeeTerminalTransactions(string ip, int userId, DateTime month);
    [OperationContract][ReferencePreservingDataContractFormat]
    
    EmployeeTerminalTransaction[] InsertEmployeeTerminalLogEntries(EmployeeTerminalLogEntry[] logEntries);
  }

}
