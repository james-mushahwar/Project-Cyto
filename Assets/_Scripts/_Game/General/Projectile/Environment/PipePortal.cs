using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Environment{

    public class PipePortal : MonoBehaviour, ITeleporter
    {
        [Header("Projectiles")]
        [SerializeField]
        private bool _allowProjectiles;
        [SerializeField]
        private float _delayProjectileOutput;
        [SerializeField]
        private Timer _delayProjectileOutputTimer;
        [SerializeField]
        private Transform _outputTransform;

        public Transform OutputTransform { get { return _outputTransform; } }

        public bool TeleportPhyiscs { get; private set; }

        [Header("IO")]
        [SerializeField]
        private PipePortal _output;

        private void Awake()
        {
            if (_output == null)
            {
                if (_allowProjectiles) 
                {
                    _output = this;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_allowProjectiles)
            {
                ITeleportEntity teleportEntity = collision.GetComponent<ITeleportEntity>();
                if (teleportEntity != null)
                {
                    Vector3 end = _output.OutputTransform.position;
                    Vector3 direction = _output.OutputTransform.up;

                    teleportEntity.Teleport(this, end, direction);
                }
            }
        }

        public void ObjectEnter()
        {

        }
    }
    
}
