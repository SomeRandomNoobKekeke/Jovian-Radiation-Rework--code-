using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Barotrauma;
using Microsoft.Xna.Framework;
using System.IO;
using System.Text;

namespace BaroJunk
{
  public class Logger
  {
    public interface ISerializer { public string Serialize(object o); }
    public class MicroSerializer : ISerializer
    {
      public string Serialize(object o)
      {
        if (o == null) return "[null]";
        if (o == "") return "[empty string]";
        return o.ToString();
      }
    }

    public static string WrapInColor(object msg, string color)
      => $"‖color:{color}‖{msg}‖end‖";
    public static string IEnumerableToString(IEnumerable<object> array)
      => $"[{String.Join(", ", array.Select(o => o?.ToString()))}]";

    public static string IDictionaryToString(System.Collections.IDictionary dict)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("{\n");
      foreach (System.Collections.DictionaryEntry entry in dict)
      {
        sb.Append($"    {entry.Key}: [{WrapInColor(entry.Value, "white")}],\n");
      }
      sb.Append("} ");

      return sb.ToString();
    }

    public Color LogColor { get; set; } = Color.Cyan;
    public Color WarningColor { get; set; } = Color.Yellow;
    public Color ErrorColor { get; set; } = Color.Red;
    public Color FunnyColor { get; set; } = Color.Magenta;

    /// <summary>
    /// Set this to true to see the source of the logs
    /// </summary>
    public bool PrintFilePath { get; set; }
    public bool PrintLogs { get; set; } = true;
    public bool PrintWarnings { get; set; } = true;
    public bool PrintErrors { get; set; } = true;

    public ISerializer Serializer { get; set; } = new MicroSerializer();


    /// <summary>
    /// Log with LogColor
    /// </summary>
    public void Log(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    { if (PrintLogs) Print(msg, LogColor, source, lineNumber); }
    public void Log(object msg1, object msg2, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      Log(msg1, LogColor, source, lineNumber);
      Log(msg2, LogColor, source, lineNumber);
    }
    public void Log(object msg1, object msg2, object msg3, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      Log(msg1, LogColor, source, lineNumber);
      Log(msg2, LogColor, source, lineNumber);
      Log(msg3, LogColor, source, lineNumber);
    }

    /// <summary>
    /// Log with WarningColor
    /// </summary>
    /// /// 
    public void Warning(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    { if (PrintWarnings) Print(msg, WarningColor, source, lineNumber); }

    /// <summary>
    /// Log with ErrorColor
    /// </summary>
    public void Error(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    { if (PrintErrors) Print(msg, ErrorColor, source, lineNumber); }

    /// <summary>
    /// Log with Color
    /// </summary>
    public void Print(object msg, Color color, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (PrintFilePath) _PrintFilePath(color, source, lineNumber);
      _Print(msg, color);
    }

    /// <summary>
    /// Log with file path
    /// </summary>
    public void Info(object msg, [CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      _PrintFilePath(LogColor * 0.8f, source, lineNumber);
      _Print(msg, LogColor);
    }

    /// <summary>
    /// Print file path and line number with funny color
    /// For debuging
    /// </summary>
    public void Point([CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
      => _PrintFilePath(FunnyColor, source, lineNumber);


    /// <summary>
    /// Print stack trace
    /// For debuging
    /// </summary>
    public void PrintStackTrace()
    {
      StackTrace st = new StackTrace(true);
      for (int i = 0; i < st.FrameCount; i++)
      {
        StackFrame sf = st.GetFrame(i);
        if (sf.GetMethod().DeclaringType is null)
        {
          Log($"-> {sf.GetMethod().DeclaringType?.Name}.{sf.GetMethod()}");
          break;
        }
        Log($"-> {sf.GetMethod().DeclaringType?.Name}.{sf.GetMethod()}");
      }
    }

    private void _Print(object msg, Color color)
    {
      LuaCsLogger.LogMessage(Serializer.Serialize(msg), color * 0.8f, color);
    }

    private void _PrintFilePath(Color color, string source, int lineNumber)
    {
      var fi = new FileInfo(source);
      LuaCsLogger.LogMessage($"{fi.Directory.Name}/{fi.Name}:{lineNumber}", color * 0.8f, color);
    }

  }
}
