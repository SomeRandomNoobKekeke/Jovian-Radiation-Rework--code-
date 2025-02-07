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
      };

      VanillaDiv["caption"] = new CUITextBlock("VANILLA")
      {
        Vertical = true,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
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
        content["vanilla"].Append(new CUITextBlock($"{pi}"));
      }


      // ---------- progress ----------
      CUIHorizontalList ProgressDiv = new CUIHorizontalList()
      {
        FitContent = new CUIBool2(false, true),
      };

      ProgressDiv["caption"] = new CUITextBlock("PRROGRESS")
      {
        Vertical = true,
        Font = GUIStyle.MonospacedFont,
        TextAlign = CUIAnchor.Center,
        Padding = new Vector2(4, 4),
      };

      ProgressDiv["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(true, false),
        FitContent = new CUIBool2(false, true),
      };

      content.Append(ProgressDiv);
      content["progress"] = ProgressDiv["content"];

      foreach (PropertyInfo pi in typeof(ProgressSettings).GetProperties())
      {
        content["progress"].Append(new CUITextBlock($"{pi}"));
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
      };

      ModDiv["content"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(true, false),
        FitContent = new CUIBool2(false, true),
      };

      content.Append(ModDiv);
      content["modsettings"] = ModDiv["content"];

      foreach (PropertyInfo pi in typeof(ModSettings).GetProperties())
      {
        content["modsettings"].Append(new CUITextBlock($"{pi}"));
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
        Absolute = new CUINullRect(0, 0, 200, 200),
        Anchor = CUIAnchor.CenterRight,
        Revealed = false,
      };

      MainFrame.Layout = new CUILayoutVerticalList();

      MainFrame["header"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),
        Direction = CUIDirection.Reverse,
        BorderColor = Color.Transparent,
      };

      MainFrame["header"]["close"] = new CUIButton("X")
      {
        Absolute = new CUINullRect(0, 0, 20, 20),
        MasterColorOpaque = Color.Red,
        AddOnMouseDown = (e) => Opened = !Opened,
        BorderColor = Color.Transparent,
      };

      MainFrame["header"]["caption"] = new CUITextBlock("Radiation Settings")
      {
        FillEmptySpace = new CUIBool2(true, false),
        TextAlign = CUIAnchor.CenterLeft,
      };

      MainFrame["nav"] = new CUIHorizontalList()
      {
        Absolute = new CUINullRect(h: 20),

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