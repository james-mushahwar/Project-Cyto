using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using _Scripts._Game.General.Spawning.AI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using DebugManager = _Scripts._Game.General.Managers.DebugManager;

namespace _Scripts.Editortools.GameObjects{
    
    [ExecuteInEditMode]
    public class IconHelper : MonoBehaviour
    {
        [SerializeField]
        private EntityIconScriptableObject _entityIconSO;

        // Update is called once per frame
        void Update()
        {
        #if UNITY_EDITOR
            UpdateIcon();
        #endif
        }

        void UpdateIcon()
        {
            Component[] comps = GetComponentsInParent(typeof(MonoBehaviour));
            foreach (Component comp in comps)
            {
                SpawnPoint sp = comp as SpawnPoint;
                if (sp != null)
                {
                    Sprite sprite = _entityIconSO.GetIcon(sp.Entity);
                    if (sprite)
                    {
                        //IconManager.SetIcon(gameObject, sprite.texture);
                        IconManager.Icon icon = IconManager.Icon.DiamondBlue;
                        switch (sp.Entity)
                        {
                            case EEntity.BombDroid:
                                icon = IconManager.Icon.DiamondOrange;
                                break;
                            default:
                                icon = IconManager.Icon.DiamondGray;
                                break;
                        }

                        IconManager.SetIcon(gameObject, icon);

                    }
                    else
                    {
                        IconManager.SetIcon(gameObject, IconManager.Icon.DiamondRed);
                    }
                }
            }
        }
    }
    
}
