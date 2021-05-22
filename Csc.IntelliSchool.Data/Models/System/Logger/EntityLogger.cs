
using Csc.Components.Common;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace Csc.IntelliSchool.Data {
  public class EntityLogger {
    public DataEntities Entities { get; set; }

    public EntityLogger(DataEntities ent) {
      Entities = ent;
    }

    #region Error
    public void LogError(User usr, SystemLogApplicationAction action, Exception ex) {
      Log(usr, SystemLogCategory.Application, SystemLogLevel.Error, action.ToString(), new SystemLogExceptionEntry(ex));
    }
    #endregion

    #region Security
    public void LogSecurity(User usr, SystemLogSecurityAction action, SystemLogDataEntryBase data) {
      Log(usr, SystemLogCategory.Security, SystemLogLevel.Information, action.ToString(), data);
    }
    #endregion

    #region Database
    public void LogDatabase(User usr, SystemLogDataAction action, Type table, int[] references, SystemLogDataEntryBase data = null, string description = null) {
      LogDatabase(usr, SystemLogLevel.Information, action, table, references, data, description);
    }

    //public void LogDatabase(User usr, SystemLogDataAction action, SystemLogDataEntry data, string description = null) {
    //  LogDatabase(usr, SystemLogLevel.Information, action, data);
    //}

    public void LogDatabase(User usr, SystemLogLevel lvl, SystemLogDataAction action, Type table, int[] references, SystemLogDataEntryBase data, string description = null) {
      Log(usr, SystemLogCategory.Data, lvl, action.ToString(), table, references, data, description);
    }
    #endregion


    #region Base
    public void Log(User usr, SystemLogCategory cat, SystemLogLevel lvl, string action = null, SystemLogDataEntryBase entryData = null) {
      Log(usr != null ? usr.UserID : new int?(), usr != null ? usr.Username : null, cat, lvl, action, null, null, entryData);
    }
    public void Log(User usr, SystemLogCategory cat, SystemLogLevel lvl, string action = null, Type table = null, int[] references = null, SystemLogDataEntryBase entryData = null, string description = null) {
      Log(usr != null ? usr.UserID : new int?(), usr != null ? usr.Username : null, cat, lvl, action, table, references, entryData, description);
    }
    public void Log(int? userId, string username, SystemLogCategory cat, SystemLogLevel lvl, string action = null, Type table = null, int[] references = null, SystemLogDataEntryBase entryData = null, string description = null) {
      SystemLog log = new SystemLog();
      log.UserID = userId;
      log.Username = username;
      log.Computer = Environment.MachineName;
      log.LogCategory = cat;
      log.LogLevel = lvl;
      log.AppVersion = AppExtensions.GetVersion();
      log.Action = action;
      if (entryData != null)
        foreach (var data in entryData.GetData()) {
          log.Data.Add(data);
        }
      if (table != null)
        log.Table = table.Name;
      if (references != null && references.Length > 0) {
        if (references.Length == 1)
          log.References = references.First().ToString();
        else
          log.References = string.Format("{0}#{1}", references.Count(), string.Join("|", references.Select(s => s.ToString())));
      }
      log.Description = description;

      Entities.SystemLogs.Add(log);
    }
    #endregion

    #region Flushing
    public void Flush() {
      Entities.SaveChanges();
    }
    #endregion
  }


}