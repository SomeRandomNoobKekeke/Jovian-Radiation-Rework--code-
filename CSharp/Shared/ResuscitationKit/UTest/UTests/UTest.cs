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
    public UTestResultBase Expected;
    public UTestResultBase Result { get; private set; }
    public string Name { get; set; } = "Unnamed";
    public bool State => Result is not null && Result.Equals(Expected);
    public string DetailsOnFail { get; set; }

    public virtual void Adapt(object realValue, object expect)
    {
      Result = realValue is UTestResultBase ? realValue as UTestResultBase : new UTestResult(expect);
      Expected = expect is UTestResultBase ? expect as UTestResultBase : new UTestResult(expect);
    }

    public UTest(object realValue, object expect, [CallerArgumentExpression("realValue")] string expression = "")
      => Init(realValue, expect, expression);
    public UTest(Func<object> method, object expect, [CallerArgumentExpression("method")] string expression = "")
    {
      ArgumentNullException.ThrowIfNull(method);

      try { Init(method.Invoke(), expect, expression); }
      catch (Exception e) { Init(new UTestError(e), expect, expression); }
    }

    private void Init(object realValue, object expect, string expression)
    {
      Name = expression.Length > TooLongForAName ? VeryLongExpressionWarning : expression;
      Adapt(realValue, expect);
    }

    public override string ToString() => $"[{UTestLogger.AsText(Name)}] {(State ? "Passed" : "Failed")} | expected: [{UTestLogger.AsText(Expected)}] got: [{UTestLogger.AsText(Result)}]{(!State && DetailsOnFail is not null ? $"({DetailsOnFail})" : "")}";

    public void Log() => UTestLogger.LogTest(this);
  }
}