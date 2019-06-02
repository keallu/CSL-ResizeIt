using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace ResizeIt
{

    public class Loading : LoadingExtensionBase
    {
        private GameObject _resizeManagerGameObject;

        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                _resizeManagerGameObject = new GameObject("ResizeItResizeManager");
                _resizeManagerGameObject.AddComponent<ResizeManager>();
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] Loading:OnLevelLoaded -> Exception: " + e.Message);
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                if (_resizeManagerGameObject != null)
                {
                    UnityEngine.Object.Destroy(_resizeManagerGameObject);
                }                
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] Loading:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}