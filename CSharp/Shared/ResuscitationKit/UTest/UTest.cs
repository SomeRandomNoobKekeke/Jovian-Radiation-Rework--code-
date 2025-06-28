using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ResuscitationKit
{
  public class UTest
  {
    public UTestResult Expected;
    public UTestResult Result { get; private set; }
    public Func<object> Method;
    public string Name { get; set; }
    public bool State => Result is not null && Result.Equals(Expected);

    public UTest Run()
    {
      try { Result = new UTestResult(Method()); }
      catch (Exception ex) { Result = new UTestError(ex); }

      return this;
    }
    public UTest(object realValue, object expect) : this(() => realValue, expect) { }
    public UTest(Func<object> method, object expect)
    {
      if (method is null) throw new ArgumentNullException(nameof(method));
      Method = method;

      Expected = expect is UTestResult ? expect as UTestResult : new UTestResult(expect);

      Run();
    }

    public override string ToString() => $"[{Name}] {(State ? "Passed" : "Failed")} | expected: [{Expected}] got: [{Result}]";

    public void Log() => UTestLogger.LogTest(this);
  }
}