using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Hazards.Lasers{
    
    public class LaserRayEntity : RaycastDamageEntity
    {
        [SerializeField]
        private bool _laserOn;
        [SerializeField]
        private bool _laserMoves;

        [SerializeField]
        private ContactFilter2D _raycastContactFilter;
        [SerializeField]
        private LayerMask _damageableLayerMask;
        [SerializeField]
        private LayerMask _blockingLayerMask;
        [SerializeField]
        private Transform _raycastStartTransform;
        [SerializeField]
        private Transform _raycastEndTransform;
        [SerializeField]
        private LineRenderer _lineRenderer;

        private Vector2 _raycastEndPosition;
        private float _raycastLength = 20.0f;
        private List<RaycastHit2D> _lastRaycastHits = new List<RaycastHit2D>();
        private List<RaycastHit2D> _filteredRaycastHits = new List<RaycastHit2D>();

        private void Awake()
        {
            if (_raycastEndTransform && _raycastStartTransform)
            {
                Vector2 startToEnd = (_raycastEndTransform.position - _raycastStartTransform.position);
                _raycastLength = startToEnd.magnitude;
            }
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            _lastRaycastHits.Clear();
            _filteredRaycastHits.Clear();

            if (_laserOn)
            {
                _raycastEndPosition = _raycastStartTransform.position + (_raycastLength * _raycastStartTransform.up);
                //RaycastHit2D hit = Physics2D.Raycast(_raycastStartTransform.position, _raycastStartTransform.up, _raycastLength);

                RaycastHit2D[] hits = Physics2D.RaycastAll(_raycastStartTransform.position, _raycastStartTransform.up, _raycastLength, _raycastContactFilter.layerMask);
                Debug.DrawRay(_raycastStartTransform.position, (_raycastLength * _raycastStartTransform.up));
                if (hits.Length > 0)
                {
                    Debug.Log("Laser hit");
                    FilterHits(hits);
                }

                _raycastEndTransform.position = _raycastEndPosition;
                _lineRenderer.SetPosition(1, _raycastEndTransform.localPosition);

                DamageHits();
            }
        }

        private void DamageHits()
        {
            for (int i = 0; i < _filteredRaycastHits.Count; i++)
            {
                RaycastHit2D hit = _filteredRaycastHits[i];
                IDamageable damageable = hit.collider.gameObject.GetComponent<IDamageable>();

                if (damageable != null)
                {
                    damageable.TakeDamage(EDamageType.Laser_Tick, EEntityType.Hazard, hit.point);
                }
            }
        }

        private void FilterHits(RaycastHit2D[] hits)
        {
            float endDistance = _raycastLength;
            RaycastHit2D _closestBlockHit = default;
            for (int i = 0; i < hits.Length; i++)
            {
                //find closest hit
                RaycastHit2D hit = hits[i];
                if ((_blockingLayerMask.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    if (hit.distance < endDistance)
                    {
                        _closestBlockHit = hit;
                        endDistance = hit.distance;
                    }
                }
            }

            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];

                if ((_damageableLayerMask.value & (1 << hit.collider.gameObject.layer)) > 0)
                {
                    if (hit.distance <= endDistance)
                    {
                        // within range of laser
                        _filteredRaycastHits.Add(hit);
                    }
                }
            }

            if (_closestBlockHit)
            {
                _raycastEndPosition = _closestBlockHit.point;
            }
                
        }
    }
    
}
