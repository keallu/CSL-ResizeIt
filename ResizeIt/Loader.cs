using ColossalFramework.UI;
using ICities;
using System;
using UnityEngine;

namespace ResizeIt
{

    public class Loader : LoadingExtensionBase
    {
        private LoadMode _loadMode;
        private GameObject _gameObject;

        public override void OnLevelLoaded(LoadMode mode)
        {
            try
            {
                _loadMode = mode;

                if (_loadMode != LoadMode.LoadGame && _loadMode != LoadMode.NewGame && _loadMode != LoadMode.NewGameFromScenario)
                {
                    return;
                }

                UIView objectOfType = UnityEngine.Object.FindObjectOfType<UIView>();
                if (objectOfType != null)
                {
                    _gameObject = new GameObject("ExpandableScrollablePanel");
                    _gameObject.transform.parent = objectOfType.transform;
                    _gameObject.AddComponent<ExpandableScrollablePanel>();
                }
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] Loader:OnLevelLoaded -> Exception: " + e.Message);
            }
        }

        public override void OnLevelUnloading()
        {
            try
            {
                if (_loadMode != LoadMode.LoadGame && _loadMode != LoadMode.NewGame && _loadMode != LoadMode.NewGameFromScenario)
                {
                    return;
                }

                if (_gameObject == null)
                {
                    return;
                }

                UnityEngine.Object.Destroy(_gameObject);
            }
            catch (Exception e)
            {
                Debug.Log("[Resize It!] Loader:OnLevelUnloading -> Exception: " + e.Message);
            }
        }
    }
}