using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.Spawning.AI{
    
    public class AISpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject _spawnPointPrefab;

        [SerializeField]
        private UnityEvent _onSpawnKilledEvent;

        public UnityEvent OnSpawnKilledEvent { get => _onSpawnKilledEvent; }

        public void CreateSpawnPoint()
        {
            GameObject spawnPoint = Instantiate(_spawnPointPrefab);
            spawnPoint.transform.parent = transform;
            spawnPoint.transform.localPosition = Vector3.zero;
        }

        private void OnEnable()
        {
            SpawnManager.Instance.AssignSpawner(this);
        }

        private void OnDisable()
        {
            SpawnManager.Instance.UnassignSpawner(this);
        }

        // Update is called once per frame
        public void Tick()
        {
            
        }

        public void OnSpawnKilled(SpawnPoint spawnPoint)
        {
            _onSpawnKilledEvent.Invoke();
        }
    }
    
}
