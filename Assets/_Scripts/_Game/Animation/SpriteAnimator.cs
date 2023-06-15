using _Scripts._Game.AI;
using _Scripts._Game.General.Managers;
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
        private Material _material;

        public SpriteRenderer Renderer { get => _renderer; }
        #endregion

        #region General
        bool _isDamageFlashing;
        [SerializeField]
        private float _damageFlashDuration = 0.5f;
        private float _damageFlashTimer;
        #endregion

        [Header("Outline shader")] 

        private float _currentXThickness;
        private float _currentYThickness;
        private IEnumerator _outlineShaderEnumerator;

        [SerializeField]
        private float _defaultXThickness = 0.0f;
        [SerializeField]
        private float _defaultYThickness = 0.0f;
        [SerializeField]
        private float _exposedXThickness = 0.5f;
        [SerializeField]
        private float _exposedYThickness = 0.5f;
        [SerializeField]
        private float _onPossessOutlineDuration = 0.25f;
        [SerializeField]
        private float _onPossessXThickness = 0.5f;
        [SerializeField]
        private float _onPossessYThickness = 0.5f;

        protected virtual void Awake()
        {
            _anim = GetComponent<Animator>();
            _renderer = GetComponent<SpriteRenderer>();
            if (_renderer)
            {
                _material = _renderer.material;
            }

            _entity = GetComponentInParent<AIEntity>();
            if (_entity)
            {
                _entity.SpriteAnimator = this;
            }
        }

        protected virtual void OnEnable()
        {
            _renderer.enabled = true;
        }

        protected virtual void OnDisable()
        {
            StopAllCoroutines();

            //reset outline shader
            OnUnexposed();
            _outlineShaderEnumerator = null;

            // reset damage flash
            _isDamageFlashing = false;
            _damageFlashTimer = 0.0f;
            _material.SetFloat("_Hit", 0);

            _renderer.enabled = false;
        }

        protected virtual void FixedUpdate()
        {
            SpriteDirection();
            MaterialUpdate();

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

        protected virtual void MaterialUpdate()
        {
            // damage flashing
            if (_isDamageFlashing)
            {
                _damageFlashTimer -= Time.deltaTime;
                if (_damageFlashTimer <= 0.0f)
                {
                    _isDamageFlashing = false;
                    _damageFlashTimer = 0.0f;

                    _material.SetFloat("_Hit", 0);
                }
            }
        }    

        public void DamageFlash()
        {
            _isDamageFlashing = true;
            _damageFlashTimer = _damageFlashDuration;

            _material.SetFloat("_Hit", 1);
        }

        public void OnPossessed()
        {
            if (_outlineShaderEnumerator != null)
            {
                StopCoroutine(_outlineShaderEnumerator);
            }
            _outlineShaderEnumerator = PossessedOutline();
            StartCoroutine(_outlineShaderEnumerator);
        }

        private IEnumerator PossessedOutline()
        {
            _material.SetFloat("_ThicknessX", _onPossessXThickness);
            _material.SetFloat("_ThicknessY", _onPossessYThickness);

            yield return TaskManager.Instance.WaitForSecondsPool.Get(_onPossessOutlineDuration);

            _material.SetFloat("_ThicknessX", _currentXThickness);
            _material.SetFloat("_ThicknessY", _currentYThickness);
        }

        public void OnExposed()
        {
            _currentXThickness = _exposedXThickness;
            _currentYThickness = _exposedYThickness;

            _material.SetFloat("_ThicknessX", _currentXThickness);
            _material.SetFloat("_ThicknessY", _currentYThickness);
        }

        public void OnUnexposed()
        {
            _currentXThickness = _defaultXThickness;
            _currentYThickness = _defaultYThickness;

            _material.SetFloat("_ThicknessX", _currentXThickness);
            _material.SetFloat("_ThicknessY", _currentYThickness);
        }
    }
    
}
