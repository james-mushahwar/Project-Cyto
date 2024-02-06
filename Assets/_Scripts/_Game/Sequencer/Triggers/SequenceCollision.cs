using _Scripts._Game.General;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Sequencer.Triggers{
    
    public class SequenceCollision : SequenceTrigger
    {
        [SerializeField]
        private Collider2D _collider2D;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject collidedGO = collision.gameObject;
            if (collidedGO)
            {
                IPossessable possessable = collidedGO.GetComponent<IPossessable>();

                bool isPlayerPossessed = PlayerEntity.Instance.gameObject == collidedGO || (PlayerEntity.Instance.Possessed == possessable);

                if (isPlayerPossessed)
                {
                    bool register = RegisterSequences();

                    if (register)
                    {
                        _collider2D.enabled = false;
                    }
                }
            }
        }
    }
    
}
