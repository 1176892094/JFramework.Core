// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2025-01-08 17:01:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.Threading.Tasks;

namespace JFramework
{
    public interface IPackHelper : IBaseHelper
    {
        Task<string> LoadServerRequest(string packName, string packUri);
        Task<byte[]> LoadPacketRequest(string packName, string packUri);
        Task<string> LoadClientRequest(string persistentData, string streamingAssets);
        Task<object> LoadAssetRequest(string persistentData, string streamingAssets);
    }
}