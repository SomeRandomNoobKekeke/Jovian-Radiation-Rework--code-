using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;

namespace ResuscitationKit
{
  public class UTestError : UTestResult
  {
    public Exception Error => Result as Exception;

    public override bool Equals(object obj)
    {
      if (obj is not UTestError error) return false;
      if (Error is null) return true;
      return Error.GetType() == error.GetType();
    }

    public UTestError(Exception ex) : base(ex) { }
  }
}