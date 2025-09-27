global using BaroJunk;
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
  public static class Utils
  {
    public static float CurrentLocationRadiationAmount()
    {
      if (
        GameMain.GameSession?.Map?.CurrentLocation == null ||
        GameMain.GameSession.Map.Radiation == null ||
        !GameMain.GameSession.Map.Radiation.Enabled ||
        GameMain.GameSession.Campaign == null
      ) { return 0; }

      return Math.Max(0,
        GameMain.GameSession.Map.Radiation.Amount
        - GameMain.GameSession.Map.CurrentLocation.MapPosition.X
      );
    }

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