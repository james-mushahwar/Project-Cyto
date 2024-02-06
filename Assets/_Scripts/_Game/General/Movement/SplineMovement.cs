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

        private void OnEnable()
        {
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
                bool shouldBePlaying = _moveEntity.GetCanMove();
                bool isPlaying = _splineAnimate.IsPlaying;

                if (shouldBePlaying != isPlaying)
                {
                    if (shouldBePlaying)
                    {
                        OnMovementEnabled();
                    }
                    else
                    {
                        OnMovementDisabled();
                    }
                }
            }
        }

        private void OnMovementEnabled()
        {
            if (!_splineAnimate.IsPlaying)
            {
                _splineAnimate.Play();
            }
        }

        private void OnMovementDisabled()
        {
            if (_splineAnimate.IsPlaying)
            {
                _splineAnimate.Pause();
            }
        }
    }
    
}
