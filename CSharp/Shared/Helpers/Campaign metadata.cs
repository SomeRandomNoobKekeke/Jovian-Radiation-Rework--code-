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
    public static void SetMetadata(string name, object value)
    {
      if (GameMain.GameSession?.GameMode is CampaignMode campaign)
      {
        campaign.CampaignMetadata.data[new Identifier(name)] = value;
        //SetDataAction.PerformOperation(campaign.CampaignMetadata, new Identifier(name), value, SetDataAction.OperationType.Set);
      }
    }

    public static object GetMetadata(string name)
    {
      if (GameMain.GameSession?.GameMode is CampaignMode campaign)
      {
        return campaign.CampaignMetadata.data.GetValueOrDefault(new Identifier(name));
      }
      return null;
    }
  }
}