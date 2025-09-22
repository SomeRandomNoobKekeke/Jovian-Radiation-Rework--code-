using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Text;

namespace BaroJunk
{
  public class TypeTree
  {
    public Type RootType;
    public TypeTreeNode RootNode => Nodes[RootType];
    public Dictionary<Type, TypeTreeNode> Nodes = new();
    public Dictionary<string, Type> TypeByName = new();
    public Dictionary<string, Type> TypeByPath = new();

    public void RunRecursive(Action<Type> action, Type start = null)
    {
      if (action is null) throw new ArgumentNullException(nameof(action));

      start ??= RootType;

      if (!Nodes.ContainsKey(start))
        throw new ArgumentException($"[{RootType}] type tree doesn't contain [{start}]");

      TypeTreeNode startNode = Nodes[start];

      action(startNode.Type);
      foreach (TypeTreeNode child in startNode.DeepChildren)
      {
        action(child.Type);
      }
    }



    public TypeTree(Type root, Assembly[] assemblies = null)
    {
      RootType = root;
      Scan(assemblies ?? new Assembly[] { Assembly.GetExecutingAssembly() });
    }

    public override string ToString()
    {
      StringBuilder sb = new StringBuilder();

      void ToStringReq(TypeTreeNode node, string offset)
      {
        sb.Append($"{offset}{node}\n");
        foreach (TypeTreeNode child in node.Children)
        {
          ToStringReq(child, offset + "|--");
        }
      }

      ToStringReq(RootNode, "");

      return sb.ToString();
    }

    private void Scan(Assembly[] assemblies)
    {
      if (RootType is null) return;

      Nodes.Clear();

      Nodes[RootType] = new TypeTreeNode(RootType);

      foreach (Assembly assembly in assemblies)
      {
        foreach (Type T in assembly.GetTypes())
        {
          if (T.IsSubclassOf(RootType))
          {
            Nodes[T] = new TypeTreeNode(T);
          }
        }
      }

      foreach (TypeTreeNode node in Nodes.Values)
      {
        if (node.Type.BaseType is null) continue;
        if (Nodes.ContainsKey(node.Type.BaseType))
        {
          Nodes[node.Type.BaseType].Children.Add(node);
          node.Parent = Nodes[node.Type.BaseType];
        }
      }

      foreach (TypeTreeNode node in Nodes.Values)
      {
        TypeByName[node.Type.Name] = node.Type;
        TypeByPath[string.Join('.', node.Path.Select(n => n.Name))] = node.Type;
      }
    }
  }

  public class TypeTreeNode
  {
    public Type Type;
    public TypeTreeNode Parent;
    public List<TypeTreeNode> Children = new List<TypeTreeNode>();
    public IEnumerable<TypeTreeNode> DeepChildren
    {
      get
      {
        foreach (TypeTreeNode child in Children)
        {
          yield return child;
          foreach (TypeTreeNode deepChild in child.DeepChildren)
          {
            yield return deepChild;
          }
        }
      }
    }

    public bool IsLeave => Children.Count == 0;
    public List<Type> Path
    {
      get
      {
        List<Type> path = new List<Type>();

        TypeTreeNode node = this;
        path.Add(node.Type);

        while (node.Parent is not null)
        {
          node = node.Parent;
          path.Add(node.Type);
        }

        path.Reverse();

        return path;
      }
    }

    public TypeTreeNode(Type T) => Type = T;
    public override string ToString() => $"[{Type}]";
  }
}
