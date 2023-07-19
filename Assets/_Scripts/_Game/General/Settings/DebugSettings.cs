using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace _Scripts._Game.General.Settings{
    [CreateAssetMenu(menuName = "Settings/DebugSettings")]
    public class DebugSettings : ScriptableObject
    {
        [Header("Damage settings")]
        [SerializeField] private bool _playerImmune = false;
        [SerializeField] private bool _enemiesImmune = false;

        public bool PlayerImmune => _playerImmune;
        public bool EnemiesImmune => _enemiesImmune;

        [Header("Movement settings")]
        [SerializeField] private bool _alwaysBondable = false;

        public bool AlwaysBondable => _alwaysBondable;

        [Header("AI Movement settings")]
        [SerializeField] private bool _aiFreezeMovement = false;

        public bool AIFreezeMovement => _aiFreezeMovement;

        [Header("Save and Load settings")]
        [SerializeField]
        private bool _saveLoadSkip = false;
        [SerializeField]
        private bool _saveLoadClear = false; // Delete all save data on start

        public bool SaveLoadSkip => _saveLoadSkip;
        public bool SaveLoadClear => _saveLoadClear;
    }
    
}
