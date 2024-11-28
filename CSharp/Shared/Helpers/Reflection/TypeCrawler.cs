using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.IO;


namespace JovianRadiationRework
{
  public static class TypeCrawler
  {
    public static Dictionary<Type, bool> PrimitiveTypes = new Dictionary<Type, bool>(){
      {typeof(int), true},
      {typeof(bool), true},
      {typeof(string), true},
      {typeof(float), true},
      {typeof(double), true},
      {typeof(Color), true},
    };

    public static Dictionary<Type, bool> AllowedComplexTypes = new Dictionary<Type, bool>(){
      {typeof(Settings), true},
      {typeof(VanillaSettings), true},
    };
  }
}