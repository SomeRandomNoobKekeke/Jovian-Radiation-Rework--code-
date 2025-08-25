using System;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;

using Barotrauma;


namespace JovianRadiationRework
{
  public class AdvancedCommandTest : UTestPack
  {
    public static AdvancedCommand TestCommand = new AdvancedCommand("bruh", "", (string[] args) => { },
      new Hints(
        new Hint("ko",
          new Hint("ko1"),
          new Hint("ko2"),
          new Hint("ko3")
        ),
        new Hint("ju",
          new Hint("ju1"),
          new Hint("ju2")
        ),
        new Hint("be",
          new Hint("be1"),
          new Hint("be2",
            new Hint("bububu")
          )
        )
      )
    );

    public class CycleTest : AdvancedCommandTest
    {
      public override void CreateTests()
      {
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko"), "bruh ju"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ju"), "bruh be"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be"), "bruh ko"));

        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko ko1"), "bruh ko ko2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko ko2"), "bruh ko ko3"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko ko3"), "bruh ko ko1"));

        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ju ju1"), "bruh ju ju2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ju ju2"), "bruh ju ju1"));
      }
    }

    public class StepTest : AdvancedCommandTest
    {
      public override void CreateTests()
      {
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh"), "bruh"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh "), "bruh ko"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko"), "bruh ju"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko "), "bruh ko ko1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko ko1"), "bruh ko ko2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko ko1 "), "bruh ko ko1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be"), "bruh ko"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be "), "bruh be be1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be be1"), "bruh be be2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be be1 "), "bruh be be1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be be2"), "bruh be be1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be be2 "), "bruh be be2 bububu"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh be be2 bububu "), "bruh be be2 bububu"));
      }
    }

    public class AutocompletionTest : AdvancedCommandTest
    {
      public override void CreateTests()
      {
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh j"), "bruh ju"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh b"), "bruh be"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko 2"), "bruh ko ko2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko k"), "bruh ko ko1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh ko 3"), "bruh ko ko3"));
      }
    }

    public class RepairTheMiddleTest : AdvancedCommandTest
    {
      public override void CreateTests()
      {
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh k 2"), "bruh ko ko2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh k k"), "bruh ko ko1"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh k 3"), "bruh ko ko3"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh u 2"), "bruh ju ju2"));
        Tests.Add(new UTest(TestCommand.AutoComplete("bruh j j"), "bruh ju ju1"));
      }
    }
  }
}