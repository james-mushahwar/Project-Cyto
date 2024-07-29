using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;

namespace _Scripts._Game.General.Spawning.AI{

    [CustomEditor(typeof(AISpawner))]
    public class AISpawnerCustomInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            AISpawner spawner = (AISpawner)target;

            if (GUILayout.Button("Create SpawnPoint"))
            {
                spawner.CreateSpawnPoint();

            }
        }
    }
    
}
