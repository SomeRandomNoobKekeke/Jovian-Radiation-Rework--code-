#define USELUA

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.IO;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using HarmonyLib;
using MoonSharp.Interpreter;

namespace CrabUI
{
  public class CUIInternalAttribute : System.Attribute { }
  [CUIInternal]
  public class CUILuaRegistrar
  {
    public static string CUITypesFile => Path.Combine(CUI.LuaFolder, "CUITypes.lua");

    public static bool IsRealCUIType(Type T)
    {
      if (T.DeclaringType != null) return false; // nested type
      if (T.Name == "<>c") return false; // guh
      if (T.IsGenericType) return false; // in lua?
      if (T.IsInterface) return false;
      if (T.IsSubclassOf(typeof(Attribute))) return false;
      if (Attribute.IsDefined(T, typeof(CUIInternalAttribute))) return false;
      if (typeof(CUILuaRegistrar).Namespace != T.Namespace) return false;
      return true;
    }



#if !USELUA
    [Conditional("DONT")]
#endif
    public void Register()
    {
      Assembly thisAssembly = Assembly.GetAssembly(typeof(CUILuaRegistrar));

      foreach (Type T in thisAssembly.GetTypes().Where(IsRealCUIType))
      {
        LuaUserData.RegisterType(T.FullName);
        // This has to be done in lua
        //GameMain.LuaCs.Lua.Globals[T.Name] = UserData.CreateStatic(T);
      }

      GameMain.LuaCs.RegisterAction<CUIInput>();
      GameMain.LuaCs.RegisterAction<float, float>();
      GameMain.LuaCs.RegisterAction<TextInputEventArgs>();
      GameMain.LuaCs.RegisterAction<string>();
      GameMain.LuaCs.RegisterAction<CUIComponent>();
      GameMain.LuaCs.RegisterAction<bool>();
      GameMain.LuaCs.RegisterAction<CUIComponent, int>();


      LuaUserData.RegisterType(typeof(CUI).FullName);
      GameMain.LuaCs.Lua.Globals[nameof(CUI)] = UserData.CreateStatic(typeof(CUI));

      //HACK
      if (Directory.Exists(CUI.LuaFolder))
      {
        ConstructLuaStaticsFile();
      }
    }

#if !USELUA
    [Conditional("DONT")]
#endif
    public void Deregister()
    {
      try
      {
        GameMain.LuaCs.Lua.Globals[nameof(CUI)] = null;
      }
      catch (Exception e)
      {
        CUI.Error(e);
      }
    }

    public void ConstructLuaStaticsFile()
    {
      Assembly thisAssembly = Assembly.GetAssembly(typeof(CUILuaRegistrar));

      string content = "-- This file is autogenerated\n";

      foreach (Type T in thisAssembly.GetTypes().Where(IsRealCUIType))
      {
        content += $"{T.Name} = LuaUserData.CreateStatic('{T.FullName}', true)\n";
      }

      using (StreamWriter writer = new StreamWriter(CUITypesFile, false))
      {
        writer.Write(content);
      }
    }
  }
}