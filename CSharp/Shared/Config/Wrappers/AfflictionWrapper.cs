using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

namespace JovianRadiationRework
{
  public class AfflictionWrapper
  {
    public AfflictionPrefab AfflictionPrefab { get; set; }

    public AfflictionWrapper() { }
    public AfflictionWrapper(AfflictionPrefab afflictionPrefab) => AfflictionPrefab = afflictionPrefab;

    //TODO, lol, wtf is this code xd
    public static AfflictionWrapper Parse(string raw)
    {
      AfflictionPrefab prefab = AfflictionPrefab.RadiationSickness;

      try
      {
        prefab = AfflictionPrefab.Prefabs[raw];
      }
      catch (Exception e)
      {
        Mod.Logger.Warning($"failed to find [{raw}] in AfflictionPrefabs, backing to RadiationSickness");
      }

      return new AfflictionWrapper(prefab);
    }

    public override string ToString() => AfflictionPrefab?.Identifier.Value;
  }



}