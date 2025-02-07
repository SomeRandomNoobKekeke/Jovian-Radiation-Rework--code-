using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Barotrauma.Networking;
using CrabUI_JovianRadiationRework;

namespace JovianRadiationRework
{
  public class SettingsUI
  {
    public CUIFrame MainFrame;

    public void SyncSettings(Settings s)
    {
      foreach (string key in Settings.flatView.Props.Keys)
      {
        try
        {
          MainFrame.DispatchDown(new CUIData(key, Settings.flatView.Get(s, key).ToString()));
        }
        catch (Exception e)
        {
          Mod.Warning(e);
        }
      }
    }

    public void FillContent(CUIComponent content)
    {
      // ---------- vanilla ----------
      CUIHorizontalList VanillaDiv = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
        Style = new CUIStyle(){
          {"BorderColor", "CUIPalette.Current.Component.Border"},
        },
      };

      VanillaDiv["caption"] = new CUITextBlock("VANILLA")
      {
        Vertical = true,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };

      VanillaDiv["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(true, false),
        FitContent = new CUIBool2(false, true),
      };

      content.Append(VanillaDiv);
      content["vanilla"] = VanillaDiv["content"];

      foreach (PropertyInfo pi in typeof(VanillaSettings).GetProperties())
      {
        CUIHorizontalList setting = CUIPrefab.InputWithValidation(pi, $"Vanilla.{pi.Name}");
        content["vanilla"].Append(setting);
      }


      // ---------- progress ----------
      CUIHorizontalList ProgressDiv = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
      };

      ProgressDiv["caption"] = new CUITextBlock("PROGRESS")
      {
        Vertical = true,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };

      ProgressDiv["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(true, false),
        FitContent = new CUIBool2(false, true),
        Style = new CUIStyle(){
          {"BorderColor", "CUIPalette.Current.Component.Border"},
        },
      };

      content.Append(ProgressDiv);
      content["progress"] = ProgressDiv["content"];

      foreach (PropertyInfo pi in typeof(ProgressSettings).GetProperties())
      {
        if (Attribute.IsDefined(pi, typeof(IgnoreAttribute))) continue;
        if (!TypeCrawler.PrimitiveTypes.ContainsKey(pi.PropertyType)) continue;
        CUIHorizontalList setting = CUIPrefab.InputWithValidation(pi, $"Mod.Progress.{pi.Name}");
        content["progress"].Append(setting);
      }


      // ---------- mod settings ----------
      CUIHorizontalList ModDiv = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
      };

      ModDiv["caption"] = new CUITextBlock("MOD\nSETTINGS")
      {
        Vertical = true,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };

      ModDiv["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(true, false),
        FitContent = new CUIBool2(false, true),
        Style = new CUIStyle(){
          {"BorderColor", "CUIPalette.Current.Component.Border"},
        },
      };

      content.Append(ModDiv);
      content["modsettings"] = ModDiv["content"];

      foreach (PropertyInfo pi in typeof(ModSettings).GetProperties())
      {
        if (Attribute.IsDefined(pi, typeof(IgnoreAttribute))) continue;
        if (!TypeCrawler.PrimitiveTypes.ContainsKey(pi.PropertyType)) continue;
        CUIHorizontalList setting = CUIPrefab.InputWithValidation(pi, $"Mod.{pi.Name}");
        content["modsettings"].Append(setting);
      }
    }

    public void OpenSettingsFrame()
    {
      MainFrame.Revealed = true;
      if (GUI.PauseMenuOpen) GUI.TogglePauseMenu();
    }

    public void CreateUI()
    {
      // CUI.Main.Global.OnKeyDown += (e) =>
      // {
      //   if (e.PressedKeys.Contains(Keys.Escape))
      //   {
      //     if (!GUI.InputBlockingMenuOpen && MainFrame.Revealed) MainFrame.Revealed = false;
      //   }
      // };

      MainFrame = new CUIFrame()
      {
        Relative = new CUINullRect(0, 0, 0.5f, 0.5f),
        Anchor = CUIAnchor.Center,
        Revealed = false,
      };

      CUI.IsBlockingPredicates.Add(() => MainFrame.Revealed);

      MainFrame["list"] = new CUIVerticalList()
      {
        Relative = new CUINullRect(0, 0, 1, 1),
      };


      CUIRadiation Radiation = new CUIRadiation(256, 256)
      {
        Relative = new CUINullRect(0, 0, 1, 1),
      };
      MainFrame["radiation"] = Radiation;
      MainFrame.OnUpdate += (t) => { if (MainFrame.Revealed) Radiation.Update(t); };



      MainFrame["list"]["header"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),
        Direction = CUIDirection.Reverse,
        BorderColor = Color.Transparent,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.H1.Background"},
          {"BorderColor", "CUIPalette.Current.H1.Border"},
        }
      };

      MainFrame["list"]["header"]["close"] = new CUICloseButton()
      {
        Absolute = new CUINullRect(0, 0, 20, 20),
        AddOnMouseDown = (e) => MainFrame.Revealed = false,
      };

      MainFrame["list"]["header"]["caption"] = new CUITextBlock("Radiation Settings")
      {
        FillEmptySpace = new CUIBool2(true, false),
        TextAlign = CUIAnchor.CenterLeft,
      };

      MainFrame["list"]["nav"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.H2.Background"},
          {"BorderColor", "CUIPalette.Current.H2.Border"},
        }
      };

      MainFrame["list"]["nav"]["save"] = new CUIButton("Save as")
      {
        FillEmptySpace = new CUIBool2(true, false),
        AddOnMouseDown = (e) => CUISaveDialog.Open(),
      };
      MainFrame["list"]["nav"]["load"] = new CUIButton("Load")
      {
        FillEmptySpace = new CUIBool2(true, false),
        AddOnMouseDown = (e) => CUIOpenDialog.Open(),
      };

      MainFrame["list"]["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(false, true),
        Scrollable = true,
        BottomGap = 0,
        ScrollSpeed = 0.3f,
      };

      FillContent(MainFrame["list"]["content"]);

      CUI.Main.Append(MainFrame);
    }

    public SettingsUI()
    {
      CreateUI();
      MainFrame.OnAnyCommand = (c) =>
      {
        Mod.Instance?.Rad_Command(new string[] { c.Name, (string)c.data });
      };

      Mod.Instance.OnSettinsChangedFromConsole += (s) =>
      {
        SyncSettings(s);
      };

    }
  }
}