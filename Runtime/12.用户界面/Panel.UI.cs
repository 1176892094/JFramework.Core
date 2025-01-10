// *********************************************************************************
// # Project: JFramework
// # Unity: 6000.3.5f1
// # Author: 云谷千羽
// # Version: 1.0.0
// # History: 2025-01-09 16:01:50
// # Recently: 2025-01-09 20:01:18
// # Copyright: 2024, 云谷千羽
// # Description: This is an automatically generated comment.
// *********************************************************************************

using UnityEngine;

namespace JFramework
{
    public abstract class UIPanel : MonoBehaviour, IEntity
    {
        public UIState state { get; set; } = UIState.Common;
        
        protected virtual void Awake()
        {
            this.Inject();
        }

        protected virtual void OnDestroy()
        {
            this.Destroy();
        }

        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}