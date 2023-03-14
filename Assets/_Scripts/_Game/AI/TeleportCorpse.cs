using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using UnityEngine;

namespace _Scripts._Game.AI{
    
    public class TeleportCorpse : Corpse
    {
        private void OnEnable()
        {
            CorpseLifetimeTimer = 0.0f;
            Rb.velocity = new Vector2(0, 0);
        }

        private void FixedUpdate()
        {
            CorpseLifetimeTimer += Time.deltaTime;
        }

        public override bool IsActive()
        {
            return CorpseLifetimeTimer < CorpseLifetime;
        }
    }
    
}
