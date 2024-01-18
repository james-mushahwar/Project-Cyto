using _Scripts._Game.General;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Prefabs.Level.Hazards.Lasers{
    
    public class LaserEntity : ContactDamageEntity
    {
        private ILogicEntity _logicEntity;

        void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
            _logicEntity.IsInputLogicValid = LogicManager.Instance.AreAllInputsValid(_logicEntity);

            if (_logicEntity.InputLogicType == ELogicType.Timed)
            {
                // create timers
            }
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }
    }
    
}
