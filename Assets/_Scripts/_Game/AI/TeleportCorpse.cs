using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using UnityEngine;

namespace _Scripts._Game.AI{
    
    public class TeleportCorpse : Corpse
    {
        private Material _fadeMaterial;

        protected override void Awake()
        {
            base.Awake();

            if (Renderer)
            {
                _fadeMaterial = Renderer.material;
            }
        }

        private void OnEnable()
        {
            CorpseLifetimeTimer = 0.0f;
            Rb.velocity = new Vector2(0, 0);
            _fadeMaterial.SetFloat("_Fade", 1.0f);
        }

        private void FixedUpdate()
        {
            CorpseLifetimeTimer += Time.deltaTime;
            float alpha = Mathf.Clamp((CorpseLifetime - CorpseLifetimeTimer) / CorpseLifetime, 0.0f, 1.0f);
            _fadeMaterial.SetFloat("_Fade", alpha);
        }

        public override bool IsActive()
        {
            return CorpseLifetimeTimer < CorpseLifetime;
        }
    }
    
}
