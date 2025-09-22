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

namespace BaroJunk
{
  public static class NetParser
  {
    public static Dictionary<Type, Action<IWriteMessage, object>> EncodeTable = new()
    {
      [typeof(bool)] = (IWriteMessage msg, object data) => msg.WriteBoolean((bool)data),
      [typeof(byte)] = (IWriteMessage msg, object data) => msg.WriteByte((byte)data),
      [typeof(UInt16)] = (IWriteMessage msg, object data) => msg.WriteUInt16((UInt16)data),
      [typeof(Int16)] = (IWriteMessage msg, object data) => msg.WriteInt16((Int16)data),
      [typeof(UInt32)] = (IWriteMessage msg, object data) => msg.WriteUInt32((UInt32)data),
      [typeof(Int32)] = (IWriteMessage msg, object data) => msg.WriteInt32((Int32)data),
      [typeof(UInt64)] = (IWriteMessage msg, object data) => msg.WriteUInt64((UInt64)data),
      [typeof(Int64)] = (IWriteMessage msg, object data) => msg.WriteInt64((Int64)data),
      [typeof(Single)] = (IWriteMessage msg, object data) => msg.WriteSingle((Single)data),
      [typeof(Double)] = (IWriteMessage msg, object data) => msg.WriteDouble((Double)data),
      [typeof(string)] = (IWriteMessage msg, object data) => msg.WriteString((string)data),
      [typeof(Identifier)] = (IWriteMessage msg, object data) => msg.WriteIdentifier((Identifier)data),
      [typeof(Color)] = (IWriteMessage msg, object data) => msg.WriteColorR8G8B8A8((Color)data),
    };

    public static SimpleResult Encode(IWriteMessage msg, ConfigEntry entry)
      => Encode(msg, entry.Value, entry.Type);
    public static SimpleResult Encode(IWriteMessage msg, object data, Type dataType)
    {
      //HACK
      if (dataType == typeof(string) && data is null)
      {
        data = Parser.NullTerm;
      }

      if (EncodeTable.ContainsKey(dataType))
      {
        EncodeTable[dataType](msg, data);
      }
      else
      {
        if (!dataType.IsPrimitive)
        {
          //Static
          MethodInfo encode = dataType.GetMethod("NetEncode", BindingFlags.Public | BindingFlags.Static);
          if (encode is not null)
          {
            try
            {
              encode.Invoke(null, new object[] { msg, data });
              return SimpleResult.Success();
            }
            catch (Exception e)
            {
              return SimpleResult.Failure($"-- NetParser couldn't encode [{dataType}] into IWriteMessage because [{e.Message}]", e);
            }
          }

          //instance
          // TODO think about putting it in a method
          encode = dataType.GetMethod("NetEncode", BindingFlags.Public | BindingFlags.Instance);

          if (encode is not null)
          {
            try
            {
              encode.Invoke(data, new object[] { msg });
              return SimpleResult.Success();
            }
            catch (Exception e)
            {
              return SimpleResult.Failure($"-- NetParser couldn't encode [{dataType}] into IWriteMessage because [{e.Message}]", e);
            }
          }


          return SimpleResult.Failure($"-- NetParser couldn't encode [{dataType}] into IWriteMessage because it doesn't have {ConfigLogger.WrapInColor($"public static void NetEncode(IWriteMessage msg, {dataType} data)", "white")} method");
        }
      }

      return SimpleResult.Failure();
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
    };

    public static SimpleResult Decode<T>(IReadMessage msg) => Decode(msg, typeof(T));
    public static SimpleResult Decode(IReadMessage msg, Type T)
    {
      if (DecodeTable.ContainsKey(T))
      {
        try
        {
          //HACK
          if (T == typeof(string))
          {
            string s = msg.ReadString();
            if (s == Parser.NullTerm) return SimpleResult.Success(null);
            return SimpleResult.Success(s);
          }
          else
          {
            return SimpleResult.Success(DecodeTable[T](msg));
          }
        }
        catch (Exception e)
        {
          return new SimpleResult()
          {
            Ok = false,
            Result = Parser.DefaultFor(T),
            Details = $"-- NetParser couldn't decode [{T}] from IReadMessage because [{e.Message}]",
            Exception = e,
          };
        }
      }
      else
      {
        MethodInfo decode = T.GetMethod("NetDecode", BindingFlags.Public | BindingFlags.Static);
        if (decode is not null)
        {
          try
          {
            return SimpleResult.Success(decode.Invoke(null, new object[] { msg }));
          }
          catch (Exception e)
          {
            return new SimpleResult()
            {
              Ok = false,
              Result = Parser.DefaultFor(T),
              Details = $"-- NetParser couldn't decode [{T}] from IReadMessage because [{e.Message}]",
              Exception = e,
            };
          }
        }

        return new SimpleResult()
        {
          Ok = false,
          Result = Parser.DefaultFor(T),
          Details = $"-- NetParser couldn't decode [{T}] from IReadMessage because [{T}] doesn't have {ConfigLogger.WrapInColor($"public static {T.Name} NetDecode(IReadMessage msg)", "white")} method",
        };
      }
    }

  }
}

