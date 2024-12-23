// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2024-12-23 18:12:21
// # Recently: 2024-12-24 01:12:43
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using System;
using System.Threading.Tasks;

namespace JFramework
{
    public interface IPoolHelper : IBaseHelper
    {
        bool IsEntity(IEntity entity);
        bool IsActive(IEntity entity);
        Task<object> Instantiate(string assetPath);
        Task<object> Instantiate(string assetPath, Type assetType);
        void OnDequeue(IEntity assetData);
        string OnEnqueue(IEntity assetData);
    }
}