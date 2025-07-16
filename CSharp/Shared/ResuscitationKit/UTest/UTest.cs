using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ResuscitationKit
{
  public class UTest
  {
    public static int TooLongForAName = 50;
    public static string VeryLongExpressionWarning = "Very long expression";
    public UTestResult Expected;
    public UTestResult Result { get; private set; }
    public Func<object> Method;
    public string Name { get; set; } = "Unnamed";
    public bool State => Result is not null && Result.Equals(Expected);

    //TODO add list UTestResult
    public UTest Run()
    {
      try { Result = new UTestResult(Method()); }
      catch (Exception ex) { Result = new UTestError(ex); }

      return this;
    }
    public UTest(object realValue, object expect, [CallerArgumentExpression("realValue")] string expression = "") : this(() => realValue, expect, expression) { }
    public UTest(Func<object> method, object expect, [CallerArgumentExpression("method")] string expression = "")
    {
      ArgumentNullException.ThrowIfNull(method);

      Method = method;

      Expected = expect is UTestResult ? expect as UTestResult : new UTestResult(expect);

      Name = expression.Length > TooLongForAName ? VeryLongExpressionWarning : expression;

      Run();
    }

    public override string ToString() => $"[{UTestLogger.AsText(Name)}] {(State ? "Passed" : "Failed")} | expected: [{UTestLogger.AsText(Expected)}] got: [{UTestLogger.AsText(Result)}]";

    public void Log() => UTestLogger.LogTest(this);
  }
}