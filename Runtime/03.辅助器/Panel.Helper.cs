// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:33
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