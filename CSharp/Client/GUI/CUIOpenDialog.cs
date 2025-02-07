using System;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using CrabUI_JovianRadiationRework;

namespace JovianRadiationRework
{
  public class CUIOpenDialog : CUIFrame
  {
    public static void Open()
    {
      CUIOpenDialog dialog = new CUIOpenDialog();
      CUI.TopMain.Append(dialog);
    }

    public CUIOpenDialog()
    {
      Layout = new CUILayoutVerticalList();
      Anchor = CUIAnchor.Center;
      Absolute = new CUINullRect(w: 200, h: 300);
      this["header"] = new CUITextBlock("Open preset:")
      {
        FitContent = new CUIBool2(false, true),
        TextScale = 1.2f,
        TextAlign = CUIAnchor.Center,
        Style = new CUIStyle(){
          {"BackgroundColor", "CUIPalette.Current.Text2.Background"},
          {"BorderColor", "CUIPalette.Current.Component.Border"},
          {"TextColor", "CUIPalette.Current.Text2.Text"},
        },
      };
      this["presets"] = new CUIVerticalList()
      {
        FillEmptySpace = new CUIBool2(false, true),
        Scrollable = true,
      };
      this["cancel"] = new CUIButton("Cancel")
      {
        AddOnMouseDown = (e) => this.RemoveSelf(),
      };

      Dictionary<string, string> allPresets = IOManager.AllPresets();
      foreach (string name in allPresets.Keys)
      {
        this["presets"].Append(new CUIButton(name)
        {
          AddOnMouseDown = (e) =>
          {
            this.RemoveSelf();
            Mod.Instance?.Rad_Load_Command(new string[] { name });
          },
        });
      }
    }
  }
}