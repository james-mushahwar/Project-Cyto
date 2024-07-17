using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Bosses.GigaBombDroid{
    
    public class GigaBombDroidCannon : MonoBehaviour
    {
        private bool _isConnected = true;
        private GigaBombDroidAIEntity _entity;
        private int _index;

        [SerializeField]
        private Rigidbody2D _rigidBody2D;

        [SerializeField]
        private float _loseCannonForce = 100.0f;
        private float _lostCannonTimerUntilDeactivate;

        public bool IsConnected { get { return _isConnected; } }

        public void Tick()
        {
            //if is falling
            if (_isConnected == false)
            {
                _lostCannonTimerUntilDeactivate += Time.deltaTime;

                if (_lostCannonTimerUntilDeactivate >= 4.0f)
                {
                    Deactivate();
                }
            }

            bool entityActive = _entity != null && _entity.isActiveAndEnabled;

            if (!entityActive)
            {
                return;
            }

            if (_isConnected && _entity.DamageState > _index)
            {
                LoseCannon(); 
            }
        }

        public void Initialise(GigaBombDroidAIEntity entity, int index)
        {
            if (_entity != null)
            {
                return;
            }

            _entity = entity;
            _index = index;
            _isConnected = true;
            _lostCannonTimerUntilDeactivate = 0.0f;
        }

        public void LoseCannon()
        {
            _isConnected = false;

            transform.SetParent(null);
            _rigidBody2D.bodyType = RigidbodyType2D.Dynamic;
            _rigidBody2D.simulated = true;
            _rigidBody2D.gravityScale = 1.0f;

            //add random force
            Vector2 direction = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
            _rigidBody2D.AddForce(direction * _loseCannonForce, ForceMode2D.Impulse);
        }

        public void Deactivate()
        {
            _rigidBody2D.bodyType = RigidbodyType2D.Static;
            _rigidBody2D.simulated = false;
            _rigidBody2D.gravityScale = 0.0f;
            gameObject.SetActive(false);

        }
    }
    
}
