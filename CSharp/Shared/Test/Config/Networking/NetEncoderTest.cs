using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;

using Barotrauma;
using ResuscitationKit;
using Barotrauma.Networking;

namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    //TODO net tests require entire utest rework
    public class NetEncoderTest : ConfigTest
    {


      public void CreateServerTests()
      {

      }

      public void CreateClientTests()
      {
        ReadWriteMessage msg0 = new ReadWriteMessage();
        msg0.WriteInt32(32);

        Tests.Add(new UTest(msg0.ReadInt32(), 32));


        ExampleConfigs.ConfigA configa = new ExampleConfigs.ConfigA();
        ExampleConfigs.ConfigA configb = new ExampleConfigs.ConfigA();
        ConfigComparison.Clear(configb);

        ReadWriteMessage msg = new ReadWriteMessage();

        NetEncoder.Encode(msg, configa);
        NetEncoder.Decode(msg, configb);

        Tests.Add(new UTest(ConfigSerialization.IsEqual(configa, configb), true));

        if (!Tests[0].State)
        {
          Mod.Log(ConfigComparison.Compare(configa, configb));
        }
      }
    }


  }
}