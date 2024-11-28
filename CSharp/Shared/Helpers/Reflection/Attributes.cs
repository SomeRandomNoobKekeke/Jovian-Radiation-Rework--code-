using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class IgnoreAttribute : System.Attribute { }
  public class IgnoreWriteAttribute : System.Attribute { }
  public class IgnoreReadAttribute : System.Attribute { }
}