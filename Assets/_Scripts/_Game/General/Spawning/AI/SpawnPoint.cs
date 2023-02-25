using _Scripts._Game.AI;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class SpawnPoint : MonoBehaviour
    {
        [Header("Spawn properties")]
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private Waypoints _waypoints;

        public Waypoints Waypoints { get => _waypoints; set => _waypoints = value; }

        private void OnEnable()
        {
            AIEntity aiEntity = AIManager.Instance.TrySpawnAI(_entity, transform.position);
            if (aiEntity != null)
            {
                aiEntity.MovementSM.Waypoints = _waypoints;
            }
            
        }
    }
    
}
