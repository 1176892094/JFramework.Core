// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-24 03:12:32
// # Recently: 2024-12-24 03:12:32
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System.IO;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IFormHelper : IBaseHelper
    {
        Task<IDataTable> Instantiate(string assetPath);

        IDataTable CreateInstance(string assetPath);

        void CreateAsset(IDataTable assetData, string assetPath);

        string Path(string objectText, FileAccess fileAccess);
    }
}