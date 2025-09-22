using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Runtime.CompilerServices;

namespace BaroJunk
{
  /// <summary>
  /// Class to track stuff that should be done once, without creating 1000 bool flags
  /// </summary>
  public static class AlreadyDone
  {
    public static HashSet<string> Paths = new();
    public static bool Once([CallerFilePath] string source = "", [CallerLineNumber] int lineNumber = 0)
    {
      if (Paths.Contains($"{source}:{lineNumber}")) return true;
      Paths.Add($"{source}:{lineNumber}");
      return false;
    }
  }
}