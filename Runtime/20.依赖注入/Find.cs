// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Reflection;

namespace JFramework
{
    public static partial class Service
    {
        public static class Find
        {
            public const BindingFlags Static = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
            public const BindingFlags Instance = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            public static Assembly Assembly(string name)
            {
                if (Service.assemblies.TryGetValue(name, out var assembly))
                {
                    return assembly;
                }

                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var current in assemblies)
                {
                    if (current.GetName().Name == name)
                    {
                        assembly = current;
                        break;
                    }
                }

                if (assembly != null)
                {
                    Service.assemblies[name] = assembly;
                }

                return assembly;
            }

            public static Type Type(string name)
            {
                if (Service.cachedType.TryGetValue(name, out var cachedType))
                {
                    return cachedType;
                }

                var index = name.LastIndexOf(',');
                if (index < 0)
                {
                    return System.Type.GetType(name);
                }

                var assembly = Assembly(name.Substring(index + 1).Trim());
                cachedType = assembly.GetType(name.Substring(0, index));
                Service.cachedType.Add(name, cachedType);
                return cachedType;
            }
        }
    }
}