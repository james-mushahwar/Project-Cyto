using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.Player;
using _Scripts._Game.AI.MovementStateMachine;
using _Scripts._Game.Animation;
using _Scripts._Game.General;

namespace _Scripts._Game.AI{
    
    public class AIEntity : MonoBehaviour, IPossessable
    {
        #region State Machines
        private AIMovementStateMachineBase _movementSM;
        private SpriteAnimator _spriteAnimator;
        #endregion

        #region AI Components
        private EnemyHealthStats _enemyHealthStats;

        public EnemyHealthStats EnemyHealthStats { get => _enemyHealthStats; }
        #endregion

        protected void Awake()
        {
            _enemyHealthStats = new EnemyHealthStats(3.0f, 3.0f);
        }

        // IPossessable
        public void OnDispossess()
        {
            // dispossess this AI
        }

        public void OnPossess()
        {
            // possess control of this AI
        }

        public HealthStats GetHealthStats()
        {
            return _enemyHealthStats;
        }
    }
    
}
