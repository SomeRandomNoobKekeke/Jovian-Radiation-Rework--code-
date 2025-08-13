using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;

using Barotrauma;

namespace JovianRadiationRework
{
  public static class OtherConfigExtensions
  {
    public static string ToText(this Dictionary<string, object> flat)
    {
      StringBuilder sb = new StringBuilder();

      sb.Append("{\n");
      foreach (string key in flat.Keys)
      {
        sb.Append($"    {key}: [{flat[key]}],\n");
      }
      sb.Append("}");

      return sb.ToString();
    }


  }


}