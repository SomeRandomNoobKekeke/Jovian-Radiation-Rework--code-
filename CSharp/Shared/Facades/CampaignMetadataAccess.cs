using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;

namespace JovianRadiationRework
{
  public static class CampaignMetadataAccess
  {
    public static void Set(string name, object value)
    {
      if (GameMain.GameSession?.GameMode is CampaignMode campaign)
      {
        campaign.CampaignMetadata.data[new Identifier(name)] = value;
      }
    }

    public static object Get(string name)
    {
      if (GameMain.GameSession?.GameMode is CampaignMode campaign)
      {
        return campaign.CampaignMetadata.data.GetValueOrDefault(new Identifier(name));
      }
      return null;
    }

    public static Dictionary<Identifier, object> Data
    {
      get
      {
        if (GameMain.GameSession?.GameMode is CampaignMode campaign)
        {
          return campaign.CampaignMetadata.data;
        }
        return null;
      }
    }
  }



}