using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;

namespace JovianRadiationRework
{
  public class Hint
  {
    public string Name { get; private set; }
    public Hint[] Children { get; private set; }

    public Hint GetChild(string name)
    {
      foreach (Hint hint in Children)
      {
        if (string.Equals(hint.Name, name, StringComparison.OrdinalIgnoreCase))
        {
          return hint;
        }
      }
      return null;
    }

    public Hint(string name, params Hint[] children) => (Name, Children) = (name, children);
  }
}