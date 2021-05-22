
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public partial class EmployeeTransactionRule {

    [IgnoreDataMember]
    [NotMapped]
    public EmployeeTransactionRuleType RuleType {
      get {
        EmployeeTransactionRuleType ruleType = EmployeeTransactionRuleType.Unknown;
        Enum.TryParse(Type, true, out ruleType);

        return ruleType;
      }
      set {
        if (value == EmployeeTransactionRuleType.Unknown)
          Type = null;
        else
          Type = value.ToString();
      }
    }
  }

}