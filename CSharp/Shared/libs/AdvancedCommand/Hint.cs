using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

using Barotrauma;

namespace BaroJunk
{
  public class Hint
  {
    public string Name { get; private set; }
    public string LowName { get; private set; }
    public List<Hint> Children { get; private set; }

    public bool HasChildren => Children.Count != 0;

    public Hint this[string key] { get => GetChild(key); }

    public Hint FindClosest(string name)
    {
      if (Children.Count == 0) return null;
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
      if (Children.Count == 0) return null;
      int index = Children.IndexOf(prev);
      if (index == -1) return Children.First();
      return Children[(index + 1) % Children.Count];
    }

    public Hint(string name, params Hint[] children)
    {
      Name = name;
      LowName = name?.ToLower();
      Children = children.ToList();
    }
    public Hint(params Hint[] children)
    {
      Children = children.ToList();
    }

    public override string ToString() => Name;

    public override bool Equals(object obj)
    {
      if (obj is not Hint other) return false;
      if (Name != other.Name) return false;
      if (Children.Count != other.Children.Count) return false;
      if (!Children.SequenceEqual(other.Children)) return false;
      return true;
    }
  }
}