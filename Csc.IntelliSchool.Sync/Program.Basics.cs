using Csc.IntelliSchool.Sync.NewModel;
using System;
using System.Linq;

namespace Csc.IntelliSchool.Sync {
  partial class Program {
    private static void SyncRules() {
      OldModel.OldEntities oldEnt = new OldModel.OldEntities();
      NewEntities newEnt = new NewModel.NewEntities();

      oldEnt.Configuration.AutoDetectChangesEnabled = false;
      oldEnt.Configuration.ProxyCreationEnabled = false;
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      newEnt.Configuration.ProxyCreationEnabled = false;

      var oldRules = oldEnt.latings.ToArray();

      foreach (var rule in oldRules) {
        newEnt.EmployeeTransactionRules.Add(new EmployeeTransactionRule() {
          Type = "In",
          Time = rule.time,
          Points = (decimal)rule.points
        });
        newEnt.EmployeeTransactionRules.Add(new EmployeeTransactionRule() {
          Type = "Out",
          Time = rule.time,
          Points = (decimal)rule.points
        });

        newEnt.EmployeeTransactionRules.Add(new EmployeeTransactionRule() {
          Type = "TimeOff",
          Time = rule.time,
          Points = (decimal)rule.points
        });
      }

      newEnt.SaveChanges();
    }

    private static void SyncShifts() {
      OldModel.OldEntities oldEnt = new OldModel.OldEntities();
      NewEntities newEnt = new NewModel.NewEntities();

      oldEnt.Configuration.AutoDetectChangesEnabled = false;
      oldEnt.Configuration.ProxyCreationEnabled = false;
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      newEnt.Configuration.ProxyCreationEnabled = false;

      var oldShifts = oldEnt.shifts.ToArray();

      foreach (var oldShift in oldShifts) {
        var shift = new EmployeeShift() {
          LocalID = oldShift.shift_id,
          Name = oldShift.shift_name,
        };

        if (oldShift.sat_in == 1) {
          shift.SaturdaysFrom = oldShift.sat_from;
          shift.SaturdaysTo = oldShift.sat_from;
        }

        if (oldShift.sun_in == 1) {
          shift.SundaysFrom = oldShift.sun_from;
          shift.SundaysTo = oldShift.sun_to;
        }

        if (oldShift.mon_in == 1) {
          shift.MondaysFrom = oldShift.mon_from;
          shift.MondaysTo = oldShift.mon_to;
        }

        if (oldShift.tue_in == 1) {
          shift.TuesdaysFrom = oldShift.tue_from;
          shift.TuesdaysTo = oldShift.tue_to;
        }

        if (oldShift.wed_in == 1) {
          shift.WednesdaysFrom = oldShift.wed_from;
          shift.WednesdaysTo = oldShift.wed_to;
        }

        if (oldShift.thr_in == 1) {
          shift.ThursdaysFrom = oldShift.thr_from;
          shift.ThursdaysTo = oldShift.thr_to;
        }

        if (oldShift.fri_in == 1) {
          shift.FridaysFrom = oldShift.fri_from;
          shift.FridaysTo = oldShift.fri_to;
        }

        newEnt.EmployeeShifts.Add(shift);
      }

      newEnt.SaveChanges();
    }

    private static void SyncTransactions(int year) {
      OldModel.OldEntities oldEnt = new OldModel.OldEntities();
      NewEntities newEnt = new NewModel.NewEntities();

      oldEnt.Configuration.AutoDetectChangesEnabled = false;
      oldEnt.Configuration.ProxyCreationEnabled = false;
      newEnt.Configuration.AutoDetectChangesEnabled = false;
      newEnt.Configuration.ProxyCreationEnabled = false;

      var oldTrans = oldEnt.employee_trans.Where(s => s.year == year).ToArray();

      Console.WriteLine("Year = {0}", year);
      int idx = 0;
      foreach (var old in oldTrans) {
        newEnt.EmployeeTerminalTransactions.Add(new EmployeeTerminalTransaction() {
          TerminalIP = old.machine_ip,
          UserID = old.user_id,
          DateTime = new DateTime(old.year, old.month, old.day, old.hour, old.minute, 0)
        });
        idx++;

        if (idx % 1000 == 0)
          Console.WriteLine(idx);
      }

      newEnt.SaveChanges();
    }
  }
}
