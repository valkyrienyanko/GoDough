using Godot;
using GoDough.Composition.Attributes;
using GoDough.Runtime;
using System;
using System.Linq;
using System.Reflection;

namespace GoDough.Composition.Extensions;

public static class NodeCompositionExtensions
{
  public static T WireNode<T>(this T node)
    where T : Node
  {
    try
    {
      Func<Attribute, bool> isInjectionAttribute = x =>
        x.GetType().IsAssignableFrom(typeof(InjectAttribute));

      var props = node.GetType().GetProperties();
      var propsWithInjection = props
        .Where(x => x.GetCustomAttributes()
          .Any(isInjectionAttribute))
        .Select(prop => new
        {
          Property = prop,
          InjectionInfo = prop.GetCustomAttributes().First(isInjectionAttribute) as InjectAttribute
        });

      foreach (var injectedPropertyInfo in propsWithInjection)
      {
        var propertyReturnType = injectedPropertyInfo.Property.GetMethod.ReturnType;
        var serviceInstance = AppHost.Instance.Application.Services.GetService(propertyReturnType);

        injectedPropertyInfo.Property
          .SetValue(node, serviceInstance);
      }

      return node;
    }
    catch (Exception ex)
    {
      GD.PrintErr(ex.ToString());
      throw ex;
    }
  }
}