using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Interactable.Shootable
{

    [RequireComponent(typeof(LogicEntity))]
    public class ShootableEntity : MonoBehaviour, IDamageable, IExposable, ISaveable
    {
        #region General
        [SerializeField]
        private int _exposeHealth;

        private ILogicEntity _logicEntity;

        private Animator _animator;
        private SpriteRenderer _renderer;
        #endregion

        //IDamageable
        public IExposable Exposable
        {
            get { return this; }
        }

        public EEntityType EntityType { get => EEntityType.Environment; }

        #region Animation
        private readonly int _startup = Animator.StringToHash("ShootButton_Startup");
        private readonly int _onIdle = Animator.StringToHash("ShootButton_OnIdle");
        private readonly int _powerDown = Animator.StringToHash("ShootButton_PowerDown");
        #endregion

        private void Awake()
        {
            _logicEntity = GetComponent<LogicEntity>();
            _logicEntity.IsInputLogicValid = LogicManager.Instance.AreAllInputsValid(_logicEntity);

            _animator = GetComponentInChildren<Animator>();
            _animator.enabled = false;
            _renderer = GetComponentInChildren<SpriteRenderer>();
        }

        private void OnEnable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }

            if (_logicEntity.UseSeparateOutputLogic)
            {
                _logicEntity.OnOutputChanged.AddListener(OnOutputChanged);
            }
            else
            {
                _logicEntity.OnInputChanged.AddListener(OnInputChanged);
            }

        }

        private void OnDisable()
        {
            if (_logicEntity == null)
            {
                _logicEntity = GetComponent<LogicEntity>();
            }

            if (_logicEntity.UseSeparateOutputLogic)
            {
                _logicEntity.OnOutputChanged.RemoveListener(OnOutputChanged);
            }
            else
            {
                _logicEntity.OnInputChanged.RemoveListener(OnInputChanged);
            }
        }

        public bool IsAlive()
        {
            return _exposeHealth > 0 && _logicEntity.IsInputLogicValid;
        }

        public bool TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            if (!IsAlive())
            {
                return false;
            }

            if (_logicEntity != null)
            {
                if (_logicEntity.UseSeparateOutputLogic == false)
                {
                    if (_logicEntity.IsInputLogicValid == false)
                    {
                        return false;
                    }
                }
            }


            bool canAcceptDamage = false;
            if (_logicEntity == null)
            {
                canAcceptDamage = true;
            }
            else
            {
                canAcceptDamage = _logicEntity.UseSeparateOutputLogic || (!_logicEntity.UseSeparateOutputLogic && _logicEntity.IsInputLogicValid);
            }


            if (canAcceptDamage)
            {
                _exposeHealth--;

                if (_exposeHealth <= 0)
                {
                    _exposeHealth = 0;
                    OnExposed();
                }
            }

            return true;
        }

        private Vector2 _damageDirection;

        public Vector2 DamageDirection
        {
            get { return _damageDirection; }
            set { _damageDirection = value; }
        }

        [SerializeField]
        private Transform _targetRoot;
        public Transform Transform
        {
            get { return _targetRoot == null ? transform : _targetRoot; }
        }

        [SerializeField]
        private List<EDamageType> _damageTypesToIgnore;
        [SerializeField]
        private List<EDamageType> _damageTypesToAccept;

        public List<EDamageType> DamageTypesToIgnore => _damageTypesToIgnore;
        public List<EDamageType> DamageTypesToAccept => _damageTypesToAccept;

        public bool IsExposed()
        {
            return _exposeHealth <= 0;
        }

        public void OnExposed()
        {
            Animate(_startup);

            _logicEntity.IsOutputLogicValid = true;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        public void FinishedStartup()
        {
            Animate(_onIdle);
        }

        public void FinishedPowerDown()
        {
            _animator.enabled = false;
        }

        public void OnUnexposed()
        {
            _exposeHealth = 1;
            Animate(_powerDown);

            _logicEntity.IsOutputLogicValid = false;
            LogicManager.Instance.OnOutputChanged(_logicEntity);
        }

        private void OnInputChanged()
        {
            if (_logicEntity.UseSeparateOutputLogic)
            {
                return;
            }

            //if (_logicEntity.IsInputLogicValid)
            //{
            //    OnExposed();
            //}
            //else
            //{
            //    if (_logicEntity.InputLogicType == ELogicType.Constant)
            //    {
            //        OnUnexposed();
            //    }
            //}
            if (!_logicEntity.IsInputLogicValid)
            {
                OnUnexposed();
            }
        }

        private void OnOutputChanged()
        {
            if (_logicEntity.IsOutputLogicValid)
            {
                OnExposed();
            }
            else
            {
                OnUnexposed();
            }
        }

        private void Animate(int hash)
        {
            _animator.enabled = true;
            _animator.CrossFade(hash, 0, 0);
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public int exposeHealth;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                exposeHealth = _exposeHealth
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _exposeHealth = saveData.exposeHealth;

            if (IsExposed())
            {
                OnExposed();
            }
            else
            {
                OnUnexposed();
            }
        }
    }

}
