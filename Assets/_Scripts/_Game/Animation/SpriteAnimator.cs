using _Scripts._Game.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Animation{
    
    public abstract class SpriteAnimator : MonoBehaviour
    {
        #region Components
        private Animator _anim;
        private AIEntity _entity;

        protected Animator Anim { get => _anim; }
        public AIEntity Entity { get => _entity; }
        #endregion

        #region States
        private int _currentState;

        protected int CurrentState { get => _currentState; set => _currentState = value; }

        protected abstract int GetState();
        protected abstract float GetSpeed(int state);
        protected abstract void SpriteDirection();
        #endregion

        #region Renderer
        private SpriteRenderer _renderer;

        public SpriteRenderer Renderer { get => _renderer; }
        #endregion

        private void OnEnable()
        {
            _renderer.enabled = true;
        }

        private void OnDisable()
        {
            _renderer.enabled = false;
        }

        protected virtual void FixedUpdate()
        {
            SpriteDirection();

            int state = GetState();
            float speed = GetSpeed(state);
            Anim.speed = speed;

            if (state == CurrentState)
            {
                return;
            }

            Anim.CrossFade(state, 0, 0);
            CurrentState = state;
        }

        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();

            _entity = GetComponentInParent<AIEntity>();
            if (_entity)
            {
                _entity.SpriteAnimator = this;
            }
        }

    }
    
}
