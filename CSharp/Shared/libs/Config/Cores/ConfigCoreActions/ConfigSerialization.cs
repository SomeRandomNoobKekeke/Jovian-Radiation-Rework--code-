using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using Barotrauma;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace BaroJunk_Config
{
  public partial class ConfigCore
  {
    public static string Beautify(XDocument doc)
    {
      StringBuilder sb = new StringBuilder();
      XmlWriterSettings settings = new XmlWriterSettings
      {
        Indent = true,
        IndentChars = "    ",
        NewLineChars = "\r\n",
        NewLineHandling = NewLineHandling.Replace,
        OmitXmlDeclaration = true,
      };
      using (XmlWriter writer = XmlWriter.Create(sb, settings))
      {
        doc.Save(writer);
      }
      return sb.ToString();
    }

    private Dictionary<string, object> ToDictOfHighlightedStrings()
    {
      Dictionary<string, object> result = new Dictionary<string, object>();

      void ToDictRec(Dictionary<string, object> dict, IConfiglike config)
      {
        foreach (ConfigEntry entry in config.GetAllEntries())
        {
          if (entry.IsConfig)
          {
            IConfiglike subConfig = config.ToConfig(entry.Value);
            if (!subConfig.IsValid) continue;
            dict[entry.Key] = new Dictionary<string, object>();
            ToDictRec(dict[entry.Key] as Dictionary<string, object>, subConfig);
          }
        }

        foreach (ConfigEntry entry in config.GetEntries())
        {
          if (!entry.IsConfig)
          {
            dict[entry.Key] = ConfigLogger.WrapInColor(entry.Value.ToString(), "white");
          }
        }
      }

      ToDictRec(result, Host);
      return result;
    }

    //FIXME this probably wont work because of json
    public string ToText()
    {
      if (Settings.PrintAsXML)
      {
        XDocument xdoc = new XDocument();
        xdoc.Add(ToXML());
        return Beautify(xdoc);
      }

      return JsonSerializer.Serialize(ToDictOfHighlightedStrings(), new JsonSerializerOptions
      {
        WriteIndented = true,
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
      });
    }

    public Dictionary<string, object> ToDict()
    {
      Dictionary<string, object> result = new Dictionary<string, object>();

      void ToDictRec(Dictionary<string, object> dict, IConfiglike config)
      {
        foreach (ConfigEntry entry in config.GetAllEntries())
        {
          if (entry.IsConfig)
          {
            IConfiglike subConfig = config.ToConfig(entry.Value);
            if (!subConfig.IsValid) continue;
            dict[entry.Key] = new Dictionary<string, object>();
            ToDictRec(dict[entry.Key] as Dictionary<string, object>, subConfig);
          }
        }

        foreach (ConfigEntry entry in config.GetEntries())
        {
          if (!entry.IsConfig)
          {
            dict[entry.Key] = entry.Value.ToString();
          }
        }
      }

      ToDictRec(result, Host);
      return result;
    }


    public XElement ToXML()
    {
      XElement ToXMLRec(XElement element, IConfiglike config)
      {
        foreach (ConfigEntry entry in config.GetAllEntries())
        {
          if (entry.IsConfig)
          {
            IConfiglike subConfig = config.ToConfig(entry.Value);
            if (!subConfig.IsValid) continue;
            element.Add(ToXMLRec(
              new XElement(entry.Key),
              subConfig
            ));
          }
        }

        foreach (ConfigEntry entry in config.GetAllEntries())
        {
          if (!entry.IsConfig)
          {
            element.Add(XMLParser.Serialize(entry.Value, entry.Key));
          }
        }

        return element;
      }

      return ToXMLRec(new XElement(Host.Name), Host);
    }

    public void FromXML(XElement element)
    {
      void fromXMLRec(XElement xml, IConfiglike config)
      {
        foreach (XElement child in xml.Elements())
        {
          ConfigEntry entry = config.GetEntry(child.Name.ToString());

          if (!entry.IsValid) continue;

          if (entry.IsConfig)
          {
            IConfiglike subConfig = Host.ToConfig(entry.Value);

            if (!subConfig.IsValid)
            {
              subConfig = Host.CreateDefaultForType(entry.Type);
              entry.Value = subConfig;
            }

            fromXMLRec(child, subConfig);
          }
          else
          {
            SimpleResult result = XMLParser.Parse(child, entry.Type);
            entry.Value = result.Result;

            if (!result.Ok) Logger.Warning(result.Details);
          }
        }
      }

      fromXMLRec(element, Host);
      Manager.ConfigLoaded();
    }

    public Func<string[][]> ToHints()
    {
      return () => new string[][] { this.GetFlat().Keys.ToArray() };
    }
  }

}