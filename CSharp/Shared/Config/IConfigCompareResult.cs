using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

namespace JovianRadiationRework
{
  public class IConfigCompareResult
  {
    public IConfig ConfigA;
    public IConfig ConfigB;
    public List<string> OnlyInA = new();
    public List<string> OnlyInB = new();
    public Dictionary<string, Tuple<object, object>> Different = new();
    public bool Equals;

    public IConfigCompareResult(IConfig A, IConfig B)
    {
      ConfigA = A;
      ConfigB = B;

      Dictionary<string, object> flatA = A.FlatValues;
      Dictionary<string, object> flatB = B.FlatValues;

      OnlyInA = flatA.Keys.Except(flatB.Keys).ToList();
      OnlyInB = flatB.Keys.Except(flatA.Keys).ToList();

      List<string> Both = flatA.Keys.Intersect(flatB.Keys).ToList();

      foreach (string key in Both)
      {
        if (!Object.Equals(flatA[key], flatB[key]))
        {
          Different[key] = new Tuple<object, object>(flatA[key], flatB[key]);
        }
      }

      Equals = OnlyInA.Count == 0 && OnlyInB.Count == 0 && Different.Count == 0;
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      sb.Append($"---------------- [ Compare result: {Equals}] ----------------\n");
      if (OnlyInA.Count > 0)
      {
        sb.Append($"---------------- [only in A] ----------------\n");
        sb.Append(OnlyInA.ToText());
      }
      if (OnlyInB.Count > 0)
      {
        sb.Append($"---------------- [only in B] ----------------\n");
        sb.Append(OnlyInB.ToText());
      }
      if (Different.Count > 0)
      {
        sb.Append($"---------------- [Different] ----------------\n");
        sb.Append(Different.ToDictionary(kp => kp.Key, kp => kp.Value.ToString() as object).ToText());
      }

      return sb.ToString();
    }
  }
}