using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Scripts._Game.General.Settings{
    [CreateAssetMenu(menuName = "Settings/DebugSettings")]
    public class DebugSettings : ScriptableSingleton<DebugSettings>
    {
        [Header("Damage settings")]
        [SerializeField] private bool _playerImmune = false;
        [SerializeField] private bool _enemiesImmune = false;

        public bool PlayerImmune => _playerImmune;
        public bool EnemiesImmune => _enemiesImmune;
    }
    
}
