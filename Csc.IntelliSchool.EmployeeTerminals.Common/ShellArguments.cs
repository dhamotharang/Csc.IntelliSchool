namespace Csc.IntelliSchool.EmployeeTerminals.Common {
  public  class ShellArguments {
    private static readonly int TerminalIDPosition = 0;
    private static readonly int LoadLogPosition = 1;
    private static readonly int ClearLogPosition = 2;
    private static readonly int AttendedPosition = 3;

    public int TerminalID { get; private set; }
    public bool LoadLog { get; private set; }
    public bool ClearLog { get; private set; }
    public bool Attended { get; private set; }

    public ShellArguments(string[] args) {
      TerminalID = int.Parse(args[TerminalIDPosition]);
      LoadLog = bool.Parse(args[LoadLogPosition]);
      ClearLog = bool.Parse(args[ClearLogPosition]);
      Attended = bool.Parse(args[AttendedPosition]);
    }


    public static bool ValidateArgs(string[] args) {
      if (args.Length != 4)
        return false;

      int tmpInt;
      bool tmpBool;

      if (int.TryParse(args[TerminalIDPosition], out tmpInt) ==false)
        return false;

      if (bool.TryParse(args[LoadLogPosition], out tmpBool) == false)
        return false;

      if (bool.TryParse(args[ClearLogPosition], out tmpBool) == false)
        return false;

      if (bool.TryParse(args[AttendedPosition], out tmpBool) == false)
        return false;

      return true;
    }

    public override string ToString() {
      return string.Format("TerminalIP={0}, LoadLog={1}, ClearLog={2}", TerminalID, LoadLog, ClearLog);
    }
  }
}
