using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Xml.Linq;
using Barotrauma.Extensions;
namespace CrabUI_JovianRadiationRework
{

  public class CUICloseButton : CUIButton
  {
    public CUICloseButton() : base()
    {
      Command = "Close";
      Text = "";
      BackgroundSprite = CUI.TextureManager.GetCUISprite(3, 1);
    }

  }
}