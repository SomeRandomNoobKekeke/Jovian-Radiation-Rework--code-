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
  public class ConfigCompareResult
  {
    public object ConfigA;
    public object ConfigB;
    public List<string> OnlyInA = new();
    public List<string> OnlyInB = new();
    public Dictionary<string, Tuple<object, object>> Different = new();
    public bool Equals;

    public ConfigCompareResult(object A, object B)
    {
      ConfigA = A;
      ConfigB = B;

      Dictionary<string, object> flatA = ConfigTraverse.GetFlatValues(A);
      Dictionary<string, object> flatB = ConfigTraverse.GetFlatValues(B);

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



      sb.Append($"----------------- [ Compare result: {Mod.WrapInColor((Equals ? "Match" : "Don't match"), Equals ? "lime" : "orange")}] -----------------\n");
      if (OnlyInA.Count > 0)
      {
        sb.Append($"------------------------ [ Only in A ] ------------------------\n");
        foreach (string key in OnlyInA)
        {
          sb.Append($"{key}\n");
        }
      }
      if (OnlyInB.Count > 0)
      {
        sb.Append($"------------------------ [ Only in B ] ------------------------\n");
        foreach (string key in OnlyInB)
        {
          sb.Append($"{key}\n");
        }
      }
      if (Different.Count > 0)
      {
        sb.Append($"------------------------ [ Different ] ------------------------\n");

        foreach (string key in Different.Keys)
        {
          sb.Append($"{key} [ {Mod.WrapInColor(Parser.Serialize(Different[key].Item1), "white")} / {Mod.WrapInColor(Parser.Serialize(Different[key].Item2), "white")} ]\n");
        }
      }

      return sb.ToString();
    }
  }
}