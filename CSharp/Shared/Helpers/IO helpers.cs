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
  public partial class Mod : IAssemblyPlugin
  {
    public void FindModFolder()
    {
      bool found = false;

      foreach (ContentPackage p in ContentPackageManager.EnabledPackages.All)
      {
        if (p.Name.Contains(ModName))
        {
          found = true;
          ModDir = Path.GetFullPath(p.Dir);
          ModVersion = p.ModVersion;
          break;
        }
      }

      if (!found) Error($"Couldn't find {ModName} mod folder");
    }
  }
}