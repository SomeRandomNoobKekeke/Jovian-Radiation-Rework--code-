using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ResuscitationKit
{
  public class UTestResult
  {
    public virtual object Result { get; set; }

    public override bool Equals(object obj)
    {
      if (obj is not UTestResult other) return false;
      return Result.Equals(other.Result);
    }

    public UTestResult(object result) => Result = result;

    public override string ToString() => Result.ToString();
  }
}