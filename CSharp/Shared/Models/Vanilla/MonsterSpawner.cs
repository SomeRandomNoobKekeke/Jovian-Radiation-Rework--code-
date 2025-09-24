using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;
using HarmonyLib;

using Barotrauma.Extensions;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Voronoi2;


namespace JovianRadiationRework
{
  public partial class VanillaRadiationModel
  {
    public class VanillaMonsterSpawner : IMonsterSpawner
    {
      public void SpawnMonsters(MonsterEvent _)
      {
        //+1 because Range returns an integer less than the max value
        int amount = Rand.Range(_.MinAmount, _.MaxAmount + 1);
        for (int i = 0; i < amount; i++)
        {
          string seed = i.ToString() + Level.Loaded.Seed;
          Character createdCharacter = Character.Create(_.SpeciesName, Vector2.Zero, seed, characterInfo: null, isRemotePlayer: false, hasAi: true, createNetworkEvent: true, throwErrorIfNotFound: false);
          if (createdCharacter == null)
          {
            DebugConsole.AddWarning($"Error in MonsterEvent: failed to spawn the character \"{_.SpeciesName}\". Content package: \"{_.prefab.ConfigElement?.ContentPackage?.Name ?? "unknown"}\".",
                _.Prefab.ContentPackage);
            _.disallowed = true;
            continue;
          }
          if (_.overridePlayDeadProbability.HasValue)
          {
            createdCharacter.EvaluatePlayDeadProbability(_.overridePlayDeadProbability);
          }
          createdCharacter.DisabledByEvent = true;
          _.monsters.Add(createdCharacter);
        }
      }


    }
  }
}