// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-19 20:12:09
// # Recently: 2024-12-22 20:12:22
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IPanelHelper : IBaseHelper
    {
        Task<object> Instantiate(string assetPath, Type assetType);

        void Destroy(IPanel assetData);

        void Surface(IPanel assetData, int index);
    }
}