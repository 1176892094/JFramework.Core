// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 23:12:53
// # Recently: 2024-12-24 01:12:39
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Form
        {
            public static async Task WriteAssets(string filePaths)
            {
                try
                {
                    var excelPaths = new List<string>();
                    var excelFiles = Directory.GetFiles(filePaths);
                    foreach (var excelFile in excelFiles)
                    {
                        if (IsSupport(excelFile))
                        {
                            excelPaths.Add(excelFile);
                        }
                    }

                    var dataTables = new Dictionary<string, List<string[]>>();
                    foreach (var excelPath in excelPaths)
                    {
                        await Task.Run(() =>
                        {
                            var assets = LoadAssets(excelPath);
                            foreach (var asset in assets)
                            {
                                if (!dataTables.ContainsKey(asset.Key))
                                {
                                    dataTables.Add(asset.Key, asset.Value);
                                }
                            }
                        });
                    }

                    var progress = 0f;
                    foreach (var data in dataTables)
                    {
                        await WriteAssets(data.Key, data.Value);
                        formHelper.CreateProgress(data.Key, ++progress / dataTables.Count);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }

            private static Dictionary<string, List<string[]>> LoadAssets(string excelPath)
            {
                var excelFile = LoadDataTable(excelPath);
                if (excelFile == null)
                {
                    return new Dictionary<string, List<string[]>>();
                }

                var dataTable = new Dictionary<string, List<string[]>>();
                foreach (var excelData in excelFile)
                {
                    var sheetName = excelData.Key;
                    var sheetData = excelData.Value;
                    var row = sheetData.GetLength(1);
                    var column = sheetData.GetLength(0);
                    var columns = new List<int>(column);
                    for (var x = 0; x < column; x++)
                    {
                        var name = sheetData[x, NAME_LINE];
                        var type = sheetData[x, TYPE_LINE];
                        if (!string.IsNullOrEmpty(name))
                        {
                            if (IsStruct(type))
                            {
                                columns.Add(x);
                            }
                            else if (IsBasic(type))
                            {
                                columns.Add(x);
                            }
                        }
                    }

                    if (columns.Count == 0)
                    {
                        continue;
                    }

                    var copies = new List<string[]>();
                    for (var y = DATA_LINE; y < row; ++y)
                    {
                        var rows = new string[columns.Count];
                        for (var x = 0; x < columns.Count; ++x)
                        {
                            var value = sheetData[columns[x], y];
                            if (value != null)
                            {
                                rows[x] = value;
                            }
                            else
                            {
                                rows[x] = string.Empty;
                            }
                        }

                        copies.Add(rows);
                    }

                    if (copies.Count > 0)
                    {
                        dataTable.Add(sheetName, copies);
                    }
                }

                return dataTable;
            }

            private static async Task WriteAssets(string sheetName, List<string[]> scriptTexts)
            {
                var filePath = Text.Format(formHelper.Path("Table", FileAccess.Write), sheetName);
                if (!File.Exists(filePath))
                {
                    return;
                }

                filePath = Path.GetDirectoryName(formHelper.Path("Data", FileAccess.Write));
                if (!string.IsNullOrEmpty(filePath) && !Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                filePath = Text.Format(formHelper.Path("Data", FileAccess.Write), sheetName);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }

                var fileName = Text.Format("JFramework.Table.{0}DataTable", sheetName);
                var fileData = (IDataTable)ScriptableObject.CreateInstance(fileName);
                if (fileData == null) return;
                var itemData = Text.Format("JFramework.Table.{0}Data,{1}", sheetName, dataAssembly);
                await Task.Run(() =>
                {
                    var instance = (IData)Activator.CreateInstance(Depend.GetType(itemData));
                    foreach (var scriptText in scriptTexts)
                    {
                        if (!string.IsNullOrEmpty(scriptText[0]))
                        {
                            instance.Create(scriptText, 0);
                            fileData.AddData(instance);
                        }
                    }
                });

                formHelper.CreateAsset(fileData, filePath);
            }
        }
    }
}