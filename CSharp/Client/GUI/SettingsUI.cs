using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Barotrauma.Networking;
using CrabUI_JovianRadiationRework;

namespace JovianRadiationRework
{
  public class SettingsUI
  {

    private bool visible; public bool Visible
    {
      get => visible;
      set
      {
        visible = value;
        opened = false;
        if (visible)
        {
          MainFrame.Revealed = false;
          OpenButton.Revealed = true;
        }
        else
        {
          MainFrame.Revealed = false;
          OpenButton.Revealed = false;
        }
      }
    }

    private bool opened; public bool Opened
    {
      get => opened;
      set
      {
        opened = value;
        visible = true;
        if (opened)
        {
          MainFrame.Revealed = true;
          OpenButton.Revealed = false;
        }
        else
        {
          MainFrame.Revealed = false;
          OpenButton.Revealed = true;
        }
      }
    }

    public CUIButton OpenButton;
    public CUIFrame MainFrame;



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
        CUIHorizontalList setting = CUIPrefab.InputWithValidation(pi, $"Progress.{pi.Name}");
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
        CUIHorizontalList setting = CUIPrefab.InputWithValidation(pi, $"Modsettings.{pi.Name}");
        content["modsettings"].Append(setting);
      }
    }

    public SettingsUI()
    {
      OpenButton = new CUIButton("RADIATION\nSETTINGS")
      {
        Vertical = true,
        Anchor = CUIAnchor.CenterRight,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
        AddOnMouseDown = (e) => Opened = !Opened,
      };
      CUI.Main.Append(OpenButton);


      MainFrame = new CUIFrame()
      {
        Relative = new CUINullRect(0, 0, 0.5f, 0.5f),
        Anchor = CUIAnchor.Center,
        Revealed = false,
      };

      MainFrame.Layout = new CUILayoutVerticalList();

      MainFrame["header"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),
        Direction = CUIDirection.Reverse,
        BorderColor = Color.Transparent,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.H1.Background"},
          {"BorderColor", "CUIPalette.Current.H1.Border"},
        }
      };

      MainFrame["header"]["close"] = new CUICloseButton()
      {
        Absolute = new CUINullRect(0, 0, 20, 20),
        AddOnMouseDown = (e) => Opened = !Opened,
      };

      MainFrame["header"]["caption"] = new CUITextBlock("Radiation Settings")
      {
        FillEmptySpace = new CUIBool2(true, false),
        TextAlign = CUIAnchor.CenterLeft,
      };

      MainFrame["nav"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.H2.Background"},
          {"BorderColor", "CUIPalette.Current.H2.Border"},
        }

      };

      MainFrame["nav"]["save"] = new CUIButton("save as")
      {

      };
      MainFrame["nav"]["load"] = new CUIButton("load")
      {

      };

      MainFrame["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(false, true),
        Scrollable = true,
      };

      FillContent(MainFrame["content"]);

      CUI.Main.Append(MainFrame);
      Opened = true;
    }
  }
}