using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace BaroJunk
{
  public class Chronometer
  {
    /// <summary>
    /// Measures execution time in TotalMilliseconds
    /// </summary>
    public static double Measure(Action action, int times = 100)
    {
      if (action is null) throw new ArgumentNullException(nameof(action));

      Stopwatch sw = new Stopwatch();
      try
      {
        // to ensure that it is compiled by JIT
        action();

        sw.Start();
        for (int i = 0; i < times; i++)
        {
          action();
        }
        sw.Stop();

        return sw.Elapsed.TotalMilliseconds / times;
      }
      catch (Exception e)
      {
        throw new Exception($"Chronometer measurement for [{action.Method.Name}] failed", e);
      }
    }
  }
}