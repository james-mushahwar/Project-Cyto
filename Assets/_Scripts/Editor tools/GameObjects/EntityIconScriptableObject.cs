using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;

namespace _Scripts.Editortools.GameObjects{

    [CreateAssetMenu(fileName = "EntityIconSO", menuName = "Editor/Icons")]
    public class EntityIconScriptableObject : ScriptableObject
    {
        [SerializeField] 
        private EntityIconDictionary _entityIconDictDictionary = new EntityIconDictionary();

        public Sprite GetIcon(EEntity entity)
        {
            Sprite sprite = null;

            _entityIconDictDictionary.TryGetValue(entity, out sprite);
            return sprite;
        }
    }
    
}
