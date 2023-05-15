using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace _Scripts._Game.Animation.Character.Player {
    
    public class PlayerSpriteAnimator : SpriteAnimator
    {
        protected override void Awake()
        {
            base.Awake();

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

        public void OnAttack(Vector2 direction)
        {

        }
    }
    
}
