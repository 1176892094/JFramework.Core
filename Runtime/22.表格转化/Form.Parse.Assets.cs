// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 23:12:53
// # Recently: 2024-12-23 23:12:53
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Form
        {
            public static void Update(string filePaths)
            {
                try
                {
                    var excelPaths = Directory.GetFiles(filePaths).Where(IsSupport).ToArray();
                    var dataTables = new Dictionary<string, List<string[]>>();
                    foreach (var excelPath in excelPaths)
                    {
                        var assets = LoadAssets(excelPath);
                        foreach (var asset in assets)
                        {
                            if (!dataTables.ContainsKey(asset.Key))
                            {
                                dataTables.Add(asset.Key, asset.Value);
                            }
                        }
                    }

                    foreach (var data in dataTables)
                    {
                        WriteAssetTask(data.Key, data.Value);
                    }
                }
                catch (Exception e)
                {
                    Error(e.ToString());
                }
            }

            private static Dictionary<string, List<string[]>> LoadAssets(string excelPath)
            {
                var excelFile = GetDataTable(excelPath);
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

                    if (columns.Count == 0) continue;
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

                    if (copies.Any())
                    {
                        dataTable.Add(sheetName, copies);
                    }
                }

                return dataTable;
            }

            private static void WriteAssetTask(string sheetName, List<string[]> scriptTexts)
            {
                if (!File.Exists(pathHelper.Path("Table", FileAccess.Write)))
                {
                    return;
                }

                if (!Directory.Exists(pathHelper.assetDataPath))
                {
                    Directory.CreateDirectory(pathHelper.assetDataPath);
                }
            }
        }
    }
}