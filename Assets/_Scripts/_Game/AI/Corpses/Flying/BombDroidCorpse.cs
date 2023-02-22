using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Corpses.Flying{
    
    public class BombDroidCorpse : Corpse
    {
        bool _isStationary = false;

        private void OnEnable()
        {
            CorpseLifetimeTimer = 0.0f;
            _isStationary = false;
            Rb.velocity = new Vector2(0, 0);
        }

        private void FixedUpdate()
        {
            CorpseLifetimeTimer += Time.deltaTime;
            if (CorpseLifetimeTimer >= CorpseLifetime && !_isStationary)
            {
                _isStationary = Rb.velocity.sqrMagnitude <= 1.0f;
                Debug.Log("Bomb droid corpse is now stationary!");
            }
        }

        public override bool IsActive()
        {
            return !_isStationary || CorpseLifetimeTimer < CorpseLifetime;
        }
    }
    
}
