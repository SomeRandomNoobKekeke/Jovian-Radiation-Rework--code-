using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

using Barotrauma;
using HarmonyLib;
using Microsoft.Xna.Framework;


namespace JovianRadiationRework
{
  public class TestContext
  {
    public string Description = "";

    public TestContext(string description = "")
    {
      Description = description;
    }

    public override string ToString() => Description;

    public static TestContext operator +(TestContext a, TestContext b)
        => new TestContext(a.Description + " | " + b.Description);
  }

  public class TestResult
  {
    public TestContext Context;
    public object Result;
    public bool? State;
    public bool Error;
    public Exception exception;

    public void ToBeEqual(object o) => State = Object.Equals(Result, o);
    public void ToBeNotEqual(object o) => State = !Object.Equals(Result, o);
    public void ToThrow() => State = Error;
    public void ToNotThrow() => State = !Error;
  }

  public class Test
  {
    public static void RunAll()
    {
      List<Type> allTest = new List<Type>();

      Assembly CallingAssembly = Assembly.GetAssembly(typeof(Test));

      foreach (Type t in CallingAssembly.GetTypes())
      {
        if (t.IsSubclassOf(typeof(Test)))
        {
          allTest.Add(t);
        }
      }

      foreach (Type T in allTest)
      {
        Run(T);
      }
    }

    public static void Run(string name)
    {
      if (String.Equals("all", name, StringComparison.OrdinalIgnoreCase))
      {
        RunAll();
        return;
      }

      Assembly CallingAssembly = Assembly.GetAssembly(typeof(Test));

      foreach (Type t in CallingAssembly.GetTypes())
      {
        if (String.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase))
        {
          Run(t);
          return;
        }
      }

      Mod.Log($"{name} not found");
    }

    public static void Run<RawType>() => Run(typeof(RawType));
    public static void Run(Type T)
    {
      if (!T.IsSubclassOf(typeof(Test)))
      {
        Mod.Log("It's not a test!");
        return;
      }


      Mod.Log($"------------------------");
      Mod.Log($"Running {T}");
      Test test = (Test)Activator.CreateInstance(T);

      test.Prepare();
      try
      {
        test.Execute();
      }
      catch (Exception e)
      {
        Mod.Error($"{T} Execution failed with:\n{e}");
      }
      finally
      {
        test.Dispose();
      }

      test.PrintResults();
    }

    public List<TestResult> Results = new List<TestResult>();
    public TestContext Context = new TestContext();


    public virtual void Execute() { }
    public virtual void Prepare() { }

    public virtual void Dispose() { }

    public void Describe(string description, Action test)
    {
      TestContext oldContext = Context;
      Context = oldContext + new TestContext(description);
      test();
      Context = oldContext;
    }


    public TestResult Expect(Action test)
    {
      TestResult result = new TestResult();
      result.Context = Context;

      try
      {
        test();
      }
      catch (Exception e)
      {
        result.Error = true;
        result.exception = e;
      }

      Results.Add(result);
      return result;
    }
    public TestResult Expect(Func<object> test)
    {
      TestResult result = new TestResult();
      result.Context = Context;

      try
      {
        result.Result = test();
      }
      catch (Exception e)
      {
        result.Error = true;
        result.exception = e;
      }

      Results.Add(result);
      return result;
    }

    public TestResult Expect(object o)
    {
      TestResult result = new TestResult();
      result.Context = Context;

      result.Result = o;

      Results.Add(result);
      return result;
    }

    public void PrintResults()
    {
      int passed = 0;
      foreach (TestResult tr in Results)
      {
        if (tr.State.HasValue && tr.State.Value) passed++;

        Color cl;
        if (tr.State.HasValue)
        {
          cl = tr.State.Value ? Color.Lime : Color.Red;
        }
        else
        {
          cl = Color.White;
        }

        object result = tr.Error ? tr.exception.Message : tr.Result;

        Mod.Log($"{tr.Context} [{result}]", cl);
      }

      string conclusion = passed == Results.Count ? "All Passed" : "Failed";

      Mod.Log($"\n{passed}/{Results.Count} {conclusion}");
    }

    public Test()
    {
      Context = new TestContext(this.GetType().Name);
    }
  }
}