// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 20:12:44
// # Recently: 2024-12-23 20:12:44
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Linq;
using System.Text;

namespace JFramework
{
    public static partial class Service
    {
        internal static partial class Table
        {
            private const int NAME_LINE = 1;
            private const int TYPE_LINE = 2;
            private const int DATA_LINE = 3;

            private static readonly string[] Basic =
            {
                "int", "long", "bool", "float", "double", "string",
            };

            private static readonly string[] Extra =
            {
                "Vector2", "Vector3", "Vector4", "Vector2Int", "Vector3Int"
            };
            
            public static bool IsBasic(string assetType)
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

                if (Basic.Any(assetType.Equals))
                {
                    return true;
                }

                if (Extra.Any(assetType.Equals))
                {
                    return true;
                }

                if (!assetType.EndsWith("[]"))
                {
                    return false;
                }

                assetType = assetType.Substring(0, assetType.IndexOf('['));
                if (Basic.Any(s => assetType.Equals(s)))
                {
                    return true;
                }

                if (Extra.Any(s => assetType.Equals(s)))
                {
                    return true;
                }

                return false;
            }

            public static bool IsStruct(string assetType)
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

            public static bool IsSupport(string assetPath)
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