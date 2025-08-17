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
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public static class NetParser
  {
    public static bool Verbose = true;

    public static void Encode(IWriteMessage msg, object data)
    {
      switch (data)
      {
        case bool: msg.WriteBoolean((bool)data); break;
        case byte: msg.WriteByte((byte)data); break;
        case Int16: msg.WriteInt16((Int16)data); break;
        case UInt16: msg.WriteUInt16((UInt16)data); break;
        case Int32: msg.WriteInt32((Int32)data); break;
        case UInt32: msg.WriteUInt32((UInt32)data); break;
        case Int64: msg.WriteInt64((Int64)data); break;
        case UInt64: msg.WriteUInt64((UInt64)data); break;
        case Single: msg.WriteSingle((Single)data); break;
        case Double: msg.WriteDouble((Double)data); break;
        case Color: msg.WriteColorR8G8B8A8((Color)data); break;
        case string: msg.WriteString((string)data); break;
        case Identifier: msg.WriteIdentifier((Identifier)data); break;
        default:
          if (!data.GetType().IsPrimitive)
          {
            MethodInfo encode = data.GetType().GetMethod("NetEncode", BindingFlags.Public | BindingFlags.Static);
            if (encode is not null)
            {
              try
              {
                encode.Invoke(null, new object[] { msg, data });
              }
              catch (Exception e)
              {
                if (Verbose)
                {
                  Mod.Warning($"-- NetParser couldn't encode [{data.GetType()}] into IWriteMessage because [{e.Message}]");
                }
              }
              return;
            }

            encode = data.GetType().GetMethod("NetEncode", BindingFlags.Public | BindingFlags.Instance);

            if (encode is not null)
            {
              try
              {
                encode.Invoke(data, new object[] { msg });
              }
              catch (Exception e)
              {
                if (Verbose)
                {
                  Mod.Warning($"-- NetParser couldn't encode [{data.GetType()}] into IWriteMessage because [{e.Message}]");
                }
              }
              return;
            }

            if (Verbose)
            {
              Mod.Warning($"-- NetParser couldn't encode [{data.GetType()}] into IWriteMessage because it doesn't have {Mod.WrapInColor("public static void NetEncode(IWriteMessage msg, {data.GetType()} data)", "white")} method");
            }
          }
          break;
      }
    }

    public static Dictionary<Type, Func<IReadMessage, object>> DecodeTable = new()
    {
      [typeof(bool)] = (IReadMessage msg) => msg.ReadBoolean(),
      [typeof(byte)] = (IReadMessage msg) => msg.ReadByte(),
      [typeof(UInt16)] = (IReadMessage msg) => msg.ReadUInt16(),
      [typeof(Int16)] = (IReadMessage msg) => msg.ReadInt16(),
      [typeof(UInt32)] = (IReadMessage msg) => msg.ReadUInt32(),
      [typeof(Int32)] = (IReadMessage msg) => msg.ReadInt32(),
      [typeof(UInt64)] = (IReadMessage msg) => msg.ReadUInt64(),
      [typeof(Int64)] = (IReadMessage msg) => msg.ReadInt64(),
      [typeof(Single)] = (IReadMessage msg) => msg.ReadSingle(),
      [typeof(Double)] = (IReadMessage msg) => msg.ReadDouble(),
      [typeof(string)] = (IReadMessage msg) => msg.ReadString(),
      [typeof(Identifier)] = (IReadMessage msg) => msg.ReadIdentifier(),
      [typeof(Color)] = (IReadMessage msg) => msg.ReadColorR8G8B8A8(),
      [typeof(bool)] = (IReadMessage msg) => msg.ReadBoolean(),
    };

    public static object Decode<T>(IReadMessage msg) => Decode(msg, typeof(T));
    public static object Decode(IReadMessage msg, Type T)
    {
      if (DecodeTable.ContainsKey(T))
      {
        try { return DecodeTable[T](msg); }
        catch (Exception e)
        {
          if (Verbose)
          {
            Mod.Warning($"-- NetParser couldn't decode [{T}] from IReadMessage because [{e.Message}]");
          }
          return T.IsPrimitive ? Activator.CreateInstance(T) : null;
        }
      }
      else
      {
        MethodInfo decode = T.GetMethod("NetDecode", BindingFlags.Public | BindingFlags.Static);
        if (decode is not null)
        {
          try
          {
            return decode.Invoke(null, new object[] { msg });
          }
          catch (Exception e)
          {
            if (Verbose)
            {
              Mod.Warning($"-- NetParser couldn't decode [{T}] from IReadMessage because [{e.Message}]");
            }
            return T.IsPrimitive ? Activator.CreateInstance(T) : null;
          }
        }

        if (Verbose)
        {
          Mod.Warning($"-- NetParser couldn't decode [{T}] from IReadMessage because [{T}] doesn't have {Mod.WrapInColor("public static object NetDecode(IReadMessage msg)", "white")} method");
        }
        return T.IsPrimitive ? Activator.CreateInstance(T) : null;
      }
    }

  }
}

