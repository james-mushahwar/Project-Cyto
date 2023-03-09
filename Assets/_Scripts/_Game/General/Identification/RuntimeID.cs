using System.Collections.Generic;
using UnityEngine;
using System;

namespace _Scripts._Game.General.Identification{
    
    public class RuntimeID : MonoBehaviour
    {
        [SerializeField]
        private string id;
        public string Id { get => id; }

        [ContextMenu("Generate id")]
        private void GenerateId()
        {
            id = Guid.NewGuid().ToString();
            Debug.Log(id);
        }

    }
    
}
