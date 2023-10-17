using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Spawning.AI;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Editortools.GameObjects{
    
    [ExecuteInEditMode]
    public class IconHelper : MonoBehaviour
    {
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
            Debug.Log("ON GUI");
            foreach (Component comp in comps)
            {
                SpawnPoint sp = comp as SpawnPoint;
                if (sp != null)
                {
                    IconManager.SetIcon(gameObject, IconManager.Icon.DiamondOrange);
                }
            }
        }
    }
    
}
