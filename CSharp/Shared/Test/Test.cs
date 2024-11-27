using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class Test
  {
    public static void Run<RawType>()
    {
      Type T = typeof(RawType);

      if (!T.IsSubclassOf(typeof(Test)))
      {
        Mod.Error("It's not a test!");
        return;
      }

      Test test = (Test)Activator.CreateInstance(T);

      test.Prepare();
      try
      {
        if (test.Execute())
        {
          Mod.Info($"{T} Passed");
        }
        else
        {
          Mod.Error($"{T} Failed");
        }
      }
      catch (Exception e)
      {
        Mod.Error($"{T} Failed with:\n{e}");
      }
      finally
      {
        test.Dispose();
      }

    }

    public virtual bool Execute() { return false; }

    public virtual void Prepare() { }

    public virtual void Dispose() { }
  }
}