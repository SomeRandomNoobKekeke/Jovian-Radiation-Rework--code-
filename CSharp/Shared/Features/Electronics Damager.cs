using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;

using System.IO;
using Barotrauma.Items.Components;


namespace JovianRadiationRework
{
  public partial class Mod : IAssemblyPlugin
  {
    public class ElectronicsDamager
    {
      public bool Debug = false;

      public List<Item> Damagable = new List<Item>();

      public bool IsValid(Item i)
      {
        bool isRepairable = i.GetComponent<Repairable>() != null;
        bool isPowerTransfer = i.GetComponent<PowerTransfer>() != null;
        bool isPowerContainer = i.GetComponent<PowerContainer>() != null;
        bool isSteering = i.GetComponent<Steering>() != null;
        bool isSonar = i.GetComponent<Sonar>() != null;

        bool isFromMainSub = i.Submarine?.Info.Type == SubmarineType.Player;

        return isFromMainSub && isRepairable && (isPowerTransfer || isPowerContainer || isSteering || isSonar);
      }

      public void FindItems()
      {
        foreach (Item i in Item.ItemList)
        {
          if (IsValid(i)) Damagable.Add(i);
        }
      }

      public void DamageItems()
      {
        if (Submarine.MainSub == null) return;

        float damage = EntityRadiationAmount(Submarine.MainSub) * settings.Mod.ElectronicsDamageMultiplier;
        damage = Math.Clamp(damage, 0, settings.Mod.MaxDamageToElectronics);

        if (Debug) Info($"Damaging Electronics {damage}");


        foreach (Item i in Damagable)
        {
          float last = i.Condition;
          i.Condition -= damage;
          if (Debug) Mod.Log($"{i}.Condition {last} -> {i.Condition}");
        }
      }
    }
  }
}