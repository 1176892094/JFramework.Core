// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 20:12:44
// # Recently: 2024-12-24 01:12:41
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;

namespace JFramework
{
    public static partial class Service
    {
        public static partial class Form
        {
            private const int NAME_LINE = 1;
            private const int TYPE_LINE = 2;
            private const int DATA_LINE = 3;

            private static readonly string[] Array =
            {
                "int", "long", "bool", "float", "double", "string",
                "Vector2", "Vector3", "Vector4", "Vector2Int", "Vector3Int"
            };

            private static bool IsBasic(string assetType)
            {
                if (string.IsNullOrEmpty(assetType))
                {
                    return false;
                }

                assetType = assetType.Trim();
                if (assetType.EndsWith(":enum"))
                {
                    return true;
                }

                foreach (var basic in Array)
                {
                    if (assetType.Equals(basic))
                    {
                        return true;
                    }
                }

                if (!assetType.EndsWith("[]"))
                {
                    return false;
                }

                assetType = assetType.Substring(0, assetType.IndexOf('['));
                foreach (var basic in Array)
                {
                    if (assetType.Equals(basic))
                    {
                        return true;
                    }
                }

                return false;
            }

            private static bool IsStruct(string assetType)
            {
                if (string.IsNullOrEmpty(assetType))
                {
                    return false;
                }

                assetType = assetType.Trim();
                if (assetType.StartsWith("{") && assetType.EndsWith("}"))
                {
                    return true;
                }

                if (assetType.StartsWith("{") && assetType.EndsWith("}[]"))
                {
                    return true;
                }

                return false;
            }

            private static bool IsSupport(string assetPath)
            {
                if (string.IsNullOrEmpty(assetPath))
                {
                    return false;
                }

                if (Path.GetFileName(assetPath).Contains("~$"))
                {
                    return false;
                }

                return Path.GetExtension(assetPath).ToLower() is ".xlsx";
            }
        }
    }
}