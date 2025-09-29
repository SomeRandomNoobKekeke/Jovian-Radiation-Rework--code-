global using BaroJunk;
using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public partial void AddCommandsProjSpecific()
    {
      AddedCommands.Add(new DebugConsole.Command("rad_printmodel", "", Rad_PrintModel_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_amount", "", Rad_Amount_Command));
      AddedCommands.Add(new DebugConsole.Command("rad_vanilla", "", Rad_Vanilla_Command,
        () => new string[][] { RadiationParamsAccess.Instance.Props.Append("reset").ToArray() }
      ));
      AddedCommands.Add(new DebugConsole.Command("campaign_metadata", "", Campaign_Metadata_Command,
        () => new string[][] { CampaignMetadataAccess.Data.Keys.Select(id => id.Value).ToArray() }
      ));
    }

    public static void Campaign_Metadata_Command(string[] args)
    {
      Mod.Logger.Log("Campaign_Metadata_Command");
    }

    public static void Rad_Vanilla_Command(string[] args)
    {
      Mod.Logger.Log("Rad_Vanilla_Command");
    }

    public static void Rad_PrintModel_Command(string[] args)
    {
      Mod.Logger.Log("Rad_PrintModel_Command");
    }

    public static void Rad_Amount_Command(string[] args)
    {
      Mod.Logger.Log("Rad_Amount_Command");
    }

  }
}