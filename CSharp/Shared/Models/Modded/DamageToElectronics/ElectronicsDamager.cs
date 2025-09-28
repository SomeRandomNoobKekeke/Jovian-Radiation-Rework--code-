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
using System.IO;
using Barotrauma.Items.Components;


namespace JovianRadiationRework
{
  public partial class DamageToElectronicsModel
  {
    public class ModdedElectronicsDamager : IElectronicsDamager
    {
      public ModelSettings Settings { get; set; }

      public double updateTimer;
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

      public void DamageElectronics(Radiation _, float deltaTime)
      {
        if (updateTimer > 0)
        {
          updateTimer -= deltaTime;
          return;
        }

        updateTimer = Settings.DamageInterval;

        if (Submarine.MainSub == null) return;

        float radAmount = Mod.CurrentModel.EntityRadAmountCalculator.CalculateAmount(
          _, Submarine.MainSub
        );

        float dps = Math.Clamp(radAmount * Settings.RadAmountToDPS, 0, Settings.MaxDPS);
        float damage = dps * Settings.DamageInterval;

        foreach (Item i in Damagable)
        {
          i.Condition -= damage;
        }
      }


      public void ScanItems()
      {
        foreach (Item i in Item.ItemList)
        {
          if (IsValid(i)) Damagable.Add(i);
        }
      }
      public void ForgetItems()
      {
        Damagable.Clear();
      }
    }
  }
}