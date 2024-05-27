using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Environment{

    [RequireComponent(typeof(LogicEntity))]
    public class PipeGroupSelector : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] _selects;

        [Header("Settings")]
        [SerializeField]
        private bool _randomIndex = false;
        [SerializeField]
        private int _indexIncrement = 1;

        private int _selectIndex = 0;

        private ILogicEntity _logicEntity;

        private void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
        }

        private void OnEnable()
        {
            if (_logicEntity != null)
            {
                _logicEntity.OnInputChanged.AddListener(OnInputChanged);
            }

        }

        private void OnDisable()
        {
            if (_logicEntity != null)
            {
                _logicEntity.OnInputChanged.RemoveListener(OnInputChanged); 
            }
        }

        public void OnInputChanged()
        {
            if (_randomIndex)
            {
                _selectIndex = Random.Range(0, _selects.Length - 1);
            }
            else
            {
                _selectIndex += _indexIncrement;
            }
        }

        public void Select()
        {
            if (_selectIndex > _selects.Length - 1)
            {
                return;
            }

            // select current index
        }
    }
    
}
