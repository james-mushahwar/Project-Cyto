using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Animation{
    
    public abstract class SpriteAnimator : MonoBehaviour
    {
        #region Components
        private Animator _anim;

        protected Animator Anim { get => _anim; }
        #endregion

        #region States
        private int _currentState;

        protected int CurrentState { get => _currentState; set => _currentState = value; }

        protected abstract int GetState();
        #endregion

        #region Renderer
        private SpriteRenderer _renderer;

        public SpriteRenderer Renderer { get => _renderer; }
        #endregion


        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
        }

    }
    
}
