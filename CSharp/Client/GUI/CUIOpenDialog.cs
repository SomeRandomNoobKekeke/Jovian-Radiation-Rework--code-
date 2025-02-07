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


    //TODO too sleepy to make a proper function
    public static bool IsVanilla(string name) => name switch
    {
      "Vanilla" => true,
      "Default" => true,
      "Hard" => true,
      "Abyssal" => true,
      _ => false,
    };

    public CUIOpenDialog()
    {
      Layout = new CUILayoutVerticalList();
      Anchor = CUIAnchor.Center;
      Resizible = false;
      Absolute = new CUINullRect(w: 300, h: 200);
      this["header"] = new CUITextBlock("Open preset:")
      {
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
        CUIHorizontalList wrapper = new CUIHorizontalList()
        {
          FitContent = new CUIBool2(false, true),
          Direction = CUIDirection.Reverse,
        };

        if (!IsVanilla(name))
        {
          wrapper["delete"] = new CUICloseButton()
          {
            CrossRelative = new CUINullRect(w: 1),
            AddOnMouseDown = (e) =>
            {
              wrapper.RemoveSelf();
              Mod.Instance?.Rad_Delete_Command(new string[] { name });
            },
          };
        }



        wrapper["open"] = new CUIButton(name)
        {
          FillEmptySpace = new CUIBool2(true, false),
          AddOnMouseDown = (e) =>
          {
            this.RemoveSelf();
            Mod.Instance?.Rad_Load_Command(new string[] { name });
          },
        };

        this["presets"].Append(wrapper);
      }
    }
  }
}