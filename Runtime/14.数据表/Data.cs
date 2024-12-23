// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 02:12:49
// # Recently: 2024-12-22 20:12:17
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace JFramework
{
    public static partial class Service
    {
        public static class Data
        {
            public static async void LoadDataTable()
            {
                if (helper == null) return;
                var filePath = pathHelper.Path("Assembly", FileAccess.Write);
                var assembly = Depend.GetAssembly(Path.GetFileNameWithoutExtension(filePath));
                if (assembly == null) return;
                var assetTypes = assembly.GetTypes().Where(type => typeof(IDataTable).IsAssignableFrom(type)).ToArray();
                if (assetTypes.Length == 0) return;
                Event.Invoke(new DataAwakeEvent(assetTypes.Select(type => type.Name).ToArray()));
                foreach (var assetType in assetTypes)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(assetType.FullName)) continue;
                        var dataTable = (IDataTable)await poolHelper.Instantiate(GetTablePath(assetType.Name));
                        var children = assembly.GetType(assetType.FullName.Substring(0, assetType.FullName.Length - 5));
                        var properties = children.GetProperties(Depend.Instance);
                        foreach (var property in properties)
                        {
                            if (property.GetCustomAttribute(typeof(PrimaryAttribute)) == null)
                            {
                                continue;
                            }

                            if (property.PropertyType == typeof(int))
                            {
                                itemTable.Add(children, LoadData<int>());
                            }
                            else if (property.PropertyType == typeof(string))
                            {
                                nameTable.Add(children, LoadData<string>());
                            }
                            else if (property.PropertyType.IsEnum)
                            {
                                enumTable.Add(children, LoadData<Enum>());
                            }

                            continue;

                            Dictionary<T, IData> LoadData<T>()
                            {
                                var items = new Dictionary<T, IData>();
                                for (var i = 0; i < dataTable.Count; i++)
                                {
                                    var data = dataTable.GetData(i);
                                    var item = (T)property.GetValue(data);
                                    if (!items.ContainsKey(item))
                                    {
                                        items.Add(item, data);
                                        continue;
                                    }

                                    Warn(Text.Format("加载数据 {0} 失败。键值重复: {1}", assetType.Name, item));
                                }

                                return items;
                            }
                        }

                        Event.Invoke(new DataUpdateEvent(assetType.Name));
                    }
                    catch (Exception e)
                    {
                        Error(Text.Format("加载 {0} 数据失败!\n{1}", assetType.Name, e));
                    }
                }

                Event.Invoke(new DataCompleteEvent());
            }

            public static T Get<T>(int key) where T : IData
            {
                if (helper == null) return default;
                if (!itemTable.TryGetValue(typeof(T), out var dataTable))
                {
                    return default;
                }

                if (!dataTable.TryGetValue(key, out var data))
                {
                    return default;
                }

                return (T)data;
            }

            public static T Get<T>(string key) where T : IData
            {
                if (helper == null) return default;
                if (!nameTable.TryGetValue(typeof(T), out var dataTable))
                {
                    return default;
                }

                if (!dataTable.TryGetValue(key, out var data))
                {
                    return default;
                }

                return (T)data;
            }

            public static T Get<T>(Enum key) where T : IData
            {
                if (helper == null) return default;
                if (!enumTable.TryGetValue(typeof(T), out var dataTable))
                {
                    return default;
                }

                if (!dataTable.TryGetValue(key, out var data))
                {
                    return default;
                }

                return (T)data;
            }

            public static T[] GetTable<T>() where T : IData
            {
                if (helper == null) return default;
                if (Service.itemTable.TryGetValue(typeof(T), out var itemTable))
                {
                    return itemTable?.Values.Cast<T>().ToArray();
                }

                if (Service.nameTable.TryGetValue(typeof(T), out var nameTable))
                {
                    return nameTable?.Values.Cast<T>().ToArray();
                }

                if (Service.enumTable.TryGetValue(typeof(T), out var enumTable))
                {
                    return enumTable?.Values.Cast<T>().ToArray();
                }

                Error(Text.Format("获取 {0} 失败!", typeof(T).Name));
                return default;
            }

            internal static void Dispose()
            {
                var itemTable = Service.itemTable.Keys.ToList();
                foreach (var data in itemTable)
                {
                    if (Service.itemTable.TryGetValue(data, out var pool))
                    {
                        pool.Clear();
                        Service.itemTable.Remove(data);
                    }
                }

                Service.itemTable.Clear();

                var enumTable = Service.enumTable.Keys.ToList();
                foreach (var data in enumTable)
                {
                    if (Service.enumTable.TryGetValue(data, out var pool))
                    {
                        pool.Clear();
                        Service.enumTable.Remove(data);
                    }
                }

                Service.enumTable.Clear();

                var nameTable = Service.nameTable.Keys.ToList();
                foreach (var data in nameTable)
                {
                    if (Service.nameTable.TryGetValue(data, out var pool))
                    {
                        pool.Clear();
                        Service.nameTable.Remove(data);
                    }
                }

                Service.nameTable.Clear();
            }
        }
    }
}