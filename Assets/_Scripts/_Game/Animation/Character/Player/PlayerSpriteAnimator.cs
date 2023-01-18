using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Animation.Character.Player {
    
    public class PlayerSpriteAnimator : SpriteAnimator
    {
        protected override void Awake()
        {
            base.Awake();

            PlayerEntity.Instance.SpriteAnimator = this;
        }

        protected override int GetState()
        {
            throw new System.NotImplementedException();
        }
    }
    
}
