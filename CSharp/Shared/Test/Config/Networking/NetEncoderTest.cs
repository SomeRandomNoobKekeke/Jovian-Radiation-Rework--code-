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
using System.Threading.Tasks;

namespace JovianRadiationRework
{
  public partial class ConfigTest : UTestPack
  {
    // TODO net tests require entire utest rework
    // there should be new base class UNetTestPack and test tree should be resolved differently
    // this test is just hardcoded for now
    public class NetEncoderTest : ConfigTest
    {

#if SERVER
      public void CreateServerTests()
      {
        GameMain.LuaCs.Networking.Receive("NetEncoderTest", (object[] args) =>
        {
          ExampleConfigs.ConfigA config = new ExampleConfigs.ConfigA();

          IReadMessage inMsg = args[0] as IReadMessage;
          Client client = args[1] as Client;

          NetEncoder.Decode(inMsg, config);

          IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("NetEncoderTest");
          NetEncoder.Encode(outMsg, config);
          GameMain.LuaCs.Networking.Send(outMsg);
        });
      }
#endif

#if CLIENT
      public void CreateClientTests()
      {
        UTest isMultiplayer = new UTest(GameMain.IsSingleplayer, false);
        Tests.Add(isMultiplayer);

        if (!isMultiplayer.Passed)return;

        ExampleConfigs.ConfigA config = ExampleConfigs.ConfigA.Filled;
        

        GameMain.LuaCs.Networking.Receive("NetEncoderTest", (object[] args) =>
        {
          ExampleConfigs.ConfigA configb = new ExampleConfigs.ConfigA();

          IReadMessage inMsg = args[0] as IReadMessage;
          NetEncoder.Decode(inMsg, configb);

          Mod.Log(configb);
          Mod.Log(config.Compare(configb));
        });

        IWriteMessage outMsg = GameMain.LuaCs.Networking.Start("NetEncoderTest");
        NetEncoder.Encode(outMsg, config);
        GameMain.LuaCs.Networking.Send(outMsg);

        Task.Delay(100).Wait();
        if (config is null)
        {
          Mod.Log($"no response from the server");
        }
      }
#endif


    }


  }
}