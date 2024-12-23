// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 20:12:35
// # Recently: 2024-12-23 20:12:58
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Xml;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Form
        {
            private static List<KeyValuePair<string, string[,]>> GetDataTable(string filePath)
            {
                var fileType = Path.GetExtension(filePath);
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var fileData = Path.GetDirectoryName(filePath);
                if (fileData == null) return null;
                fileData = Path.Combine(fileData, Text.Format("{0}_TMP{1}", fileName, fileType));
                File.Copy(filePath, fileData, true);
                try
                {
                    using var stream = File.OpenRead(fileData);
                    using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
                    var sheetName = LoadSheetName(archive);
                    var sharedString = LoadSharedString(archive);
                    var dataTable = new List<KeyValuePair<string, string[,]>>();
                    for (var i = 0; i < sheetName.Count; i++)
                    {
                        var worksheet = GetWorksheet(archive, sharedString, i);
                        dataTable.Add(new KeyValuePair<string, string[,]>(sheetName[i], worksheet));
                    }

                    return dataTable;
                }
                finally
                {
                    File.Delete(fileData);
                }
            }

            private static XmlDocument GetDocument(ZipArchive archive, string name)
            {
                var zipEntry = archive.GetEntry(name);
                var document = new XmlDocument();
                if (zipEntry != null)
                {
                    using var stream = zipEntry.Open();
                    document.Load(stream);
                }

                return document;
            }

            private static List<string> LoadSheetName(ZipArchive archive)
            {
                var document = GetDocument(archive, "xl/workbook.xml");
                var manager = new XmlNamespaceManager(document.NameTable);
                manager.AddNamespace("x", "http://schemas.openxmlformats.org/spreadsheetml/2006/main");
                var childNodes = document.SelectNodes("//x:sheet", manager);
                var sheetName = new List<string>();
                if (childNodes != null)
                {
                    foreach (XmlNode childNode in childNodes)
                    {
                        if (childNode.Attributes != null)
                        {
                            sheetName.Add(childNode.Attributes["name"].Value);
                        }
                    }
                }

                return sheetName;
            }

            private static List<string> LoadSharedString(ZipArchive archive)
            {
                var document = GetDocument(archive, "xl/sharedStrings.xml");
                var sharedString = new List<string>();
                if (document.DocumentElement != null)
                {
                    foreach (XmlNode childNode in document.DocumentElement.ChildNodes)
                    {
                        if (childNode["t"] != null)
                        {
                            sharedString.Add(childNode["t"].InnerText);
                        }
                    }
                }

                return sharedString;
            }

            private static string[,] GetWorksheet(ZipArchive archive, List<string> sharedStrings, int i)
            {
                var document = GetDocument(archive, Text.Format("xl/worksheets/sheet{0}.xml", i + 1));
                var childNodes = document.GetElementsByTagName("sheetData")[0].ChildNodes;
                if (childNodes.Count == 0)
                {
                    return null;
                }

                var column = GetDimensions(childNodes[0]);
                if (column == 0)
                {
                    return null;
                }

                var dataTable = new string[column, childNodes.Count];
                SetWorksheet(dataTable, childNodes, sharedStrings);
                return dataTable;
            }

            private static int GetDimensions(XmlNode node)
            {
                var column = 0;
                var childNode = node.Attributes?["spans"].Value.Split(':')[1];
                if (childNode != null)
                {
                    column = int.Parse(childNode);
                }

                return column;
            }

            private static void SetWorksheet(string[,] dataTable, XmlNodeList childNodes, IReadOnlyList<string> sharedStrings)
            {
                var rowCount = dataTable.GetLength(1);
                var columnCount = dataTable.GetLength(0);
                foreach (XmlNode rowNode in childNodes)
                {
                    var value = rowNode.Attributes?["r"].Value;
                    if (value == null) continue;
                    var row = int.Parse(value) - 1;

                    foreach (XmlNode columnNode in rowNode.ChildNodes)
                    {
                        var message = columnNode["v"]?.InnerText;
                        if (columnNode.Attributes?["t"]?.Value == "s")
                        {
                            if (int.TryParse(message, out var index))
                            {
                                message = sharedStrings[index];
                            }
                        }

                        var column = GetDimensions(columnNode.Attributes?["r"]?.Value);
                        if (column >= 0 && column < columnCount && row >= 0 && row < rowCount)
                        {
                            dataTable[column, row] = message;
                        }
                    }
                }
            }

            private static int GetDimensions(string node)
            {
                var builder = new StringBuilder(1024);
                foreach (var value in node)
                {
                    if (char.IsLetter(value))
                    {
                        builder.Append(value);
                    }
                }

                var result = 0;
                var reason = builder.ToString();
                foreach (var c in reason)
                {
                    result = result * 26 + (c - 'A') + 1;
                }

                var column = result - 1;
                builder.Clear();
                return column;
            }
        }
    }
}