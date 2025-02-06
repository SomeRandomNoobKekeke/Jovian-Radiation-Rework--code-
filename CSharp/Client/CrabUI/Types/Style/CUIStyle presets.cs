using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using Barotrauma;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Xml;
using System.Xml.Linq;

namespace CrabUI_JovianRadiationRework
{

  /// <summary>
  /// In Fact just an observable dict
  /// </summary>
  public partial class CUIStyle : IEnumerable<KeyValuePair<string, string>>
  {
    public static CUIStyle Invisible => new CUIStyle(){
      {"BackgroundColor", "Transparent"},
      {"BorderColor", "Transparent"},
    };


  }



}