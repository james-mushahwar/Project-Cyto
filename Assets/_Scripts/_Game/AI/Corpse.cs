using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI{
    
    public abstract class Corpse : MonoBehaviour
    {
        [SerializeField]
        private EEntity _entity;
        [SerializeField]
        private float _corpseLifetime;
        private float _corpseLifetimeTimer;

        public float CorpseLifetime { get => _corpseLifetime; }
        public float CorpseLifetimeTimer { get => _corpseLifetimeTimer; set => _corpseLifetimeTimer = value; }

        [Header("Components")]
        private Rigidbody2D _rb;
        private Animator _deathAnimation;
        private SpriteRenderer _renderer;

        public Rigidbody2D Rb { get => _rb; }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _deathAnimation = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
        }

        public void TestFunction()
        {
            Debug.Log("HEy yall");
        }

        public abstract bool IsActive();
    }
    
}
