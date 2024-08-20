using _Scripts._Game.General.LogicController;
using _Scripts._Game.General.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

namespace _Scripts._Game.General.Movement{
    
    public class SplineMovement : MonoBehaviour
    {
        [SerializeField]
        private IMoveableEntity _moveEntity;
        [SerializeField]
        private SplineAnimate _splineAnimate;
        private bool _cachedSplinePlayChanged;

        [SerializeField]
        private LogicEntity _onSplineMovementStarted;
        [SerializeField]
        private LogicEntity _onSplineMovementStopped;

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            if (_splineAnimate)
            {
                _cachedSplinePlayChanged = _splineAnimate.IsPlaying;
            }

            if (_moveEntity == null)
            {
                _moveEntity = GetComponentInChildren<IMoveableEntity>(true);
            }

            if (_moveEntity != null && _splineAnimate != null)
            {
                //_moveEntity.MoveEnabled.AddListener(OnMovementEnabled);
                //_moveEntity.MoveDisabled.AddListener(OnMovementDisabled);
            }
        }

        private void OnDisable()
        {
            if (_moveEntity != null && _splineAnimate != null)
            {
                //_moveEntity.MoveEnabled.RemoveListener(OnMovementEnabled);
                //_moveEntity.MoveDisabled.RemoveListener(OnMovementDisabled);
            }
        }

        private void Update()
        {
            if (_moveEntity != null)
            {
                bool canPlay = _moveEntity.GetCanMove() && (_splineAnimate.Loop != SplineAnimate.LoopMode.Once || (_splineAnimate.Loop == SplineAnimate.LoopMode.Once && _splineAnimate.NormalizedTime < 1.0f));

                bool isPlaying = _splineAnimate.IsPlaying;
                bool splinePlayingChanged = _splineAnimate.IsPlaying != _cachedSplinePlayChanged;

                if (!isPlaying)
                {
                    if (canPlay)
                    {
                        OnMovementEnabled();
                    }
                    else if (splinePlayingChanged)
                    {
                        OnMovementDisabled();
                    }
                }

                _cachedSplinePlayChanged = isPlaying;
            }
        }

        private void OnMovementEnabled()
        {
            if (!_splineAnimate.IsPlaying)
            {
                _splineAnimate.Play();
            }

            if (_onSplineMovementStarted)
            {
                _onSplineMovementStarted.IsOutputLogicValid = true;
                LogicManager.Instance.OnOutputChanged(_onSplineMovementStarted);
                _onSplineMovementStarted.IsOutputLogicValid = false; // pulse signal
            }
        }

        private void OnMovementDisabled()
        {
            if (_splineAnimate.IsPlaying)
            {
                _splineAnimate.Pause();
            }

            if (_onSplineMovementStopped)
            {
                _onSplineMovementStopped.IsOutputLogicValid = true;
                LogicManager.Instance.OnOutputChanged(_onSplineMovementStopped);
                _onSplineMovementStopped.IsOutputLogicValid = false; // pulse signal
            }
        }
    }
    
}
