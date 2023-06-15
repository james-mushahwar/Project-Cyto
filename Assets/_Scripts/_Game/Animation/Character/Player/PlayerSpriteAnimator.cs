using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts._Game.Animation.Character.Player {
    
    public class PlayerSpriteAnimator : SpriteAnimator
    {
        #region Components
        private TrailRenderer _trail;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _trail = GetComponent<TrailRenderer>();
            PlayerEntity.Instance.SpriteAnimator = this;
        }

        protected override void FixedUpdate()
        {
            //base.FixedUpdate();
        }

        protected override float GetSpeed(int state)
        {
            return 1.0f;
        }

        protected override int GetState()
        {
            return 0;
        }

        protected override void SpriteDirection()
        {
            
        }

        protected override void OnEnable()
        {
            _trail.enabled = true;
            base.OnEnable();
        }

        protected override void OnDisable()
        {
            _trail.enabled = false;
            base.OnDisable();
        }
    }
    
}
