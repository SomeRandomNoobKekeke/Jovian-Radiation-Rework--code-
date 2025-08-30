using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace JovianRadiationRework
{
  public static class XMLParser
  {
    public static T Parse<T>(XElement element) => (T)Parse(element, typeof(T));
    public static object Parse(XElement element, Type T)
    {
      MethodInfo fromxml = T.GetMethod("FromXML", BindingFlags.Public | BindingFlags.Instance);
      if (fromxml != null) return fromxml.Invoke(null, new object[] { element });

      if (ExtraXMLParsingMethods.Parse.ContainsKey(T))
      {
        return ExtraXMLParsingMethods.Parse[T].Invoke(null, new object[] { element });
      }

      return Parser.Parse(element.Value, T);
    }


    public static XElement Serialize(object o, string name)
    {
      if (o is null) return new XElement(name, Parser.Serialize(o));

      MethodInfo toxml = o.GetType().GetMethod("ToXml", BindingFlags.Public | BindingFlags.Instance);
      if (toxml != null) return (XElement)toxml.Invoke(o, new object[] { });

      if (ExtraXMLParsingMethods.Serialize.ContainsKey(o.GetType()))
      {
        return (XElement)ExtraXMLParsingMethods.Serialize[o.GetType()].Invoke(null, new object[] { o });
      }

      return new XElement(name, Parser.Serialize(o));
    }
  }
}
