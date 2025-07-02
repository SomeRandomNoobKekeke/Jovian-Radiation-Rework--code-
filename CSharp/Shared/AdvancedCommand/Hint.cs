using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;

namespace JovianRadiationRework
{
  public class Hints
  {
    public Hint[] Children { get; private set; }

    public bool HasChildren => Children.Length != 0;

    public Hint FindClosest(string name)
    {
      if (Children.Length == 0) return null;
      if (string.IsNullOrEmpty(name)) return null;

      string lowName = name.ToLower();

      return Children.First(hint => hint.LowName.Contains(lowName));
    }

    public Hint GetChild(string name)
    {
      if (name is null) return null;

      foreach (Hint hint in Children)
      {
        if (string.Equals(hint.Name, name, StringComparison.OrdinalIgnoreCase))
        {
          return hint;
        }
      }
      return null;
    }

    public Hint First() => Children.FirstOrDefault();

    public Hint Next(Hint prev)
    {
      if (Children.Length == 0) return null;
      int index = Children.IndexOf(prev);
      if (index == -1) return Children.First();
      return Children[(index + 1) % Children.Length];
    }

    public Hints(params Hint[] children) => (Children) = (children);
  }

  public class Hint : Hints
  {
    public string Name { get; private set; }
    public string LowName { get; private set; }

    public Hint(string name, params Hint[] children) : base(children)
    {
      Name = name;
      LowName = name.ToLower();
    }

    public override string ToString() => Name;
  }
}