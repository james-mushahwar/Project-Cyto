using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedCameraBounds : BaseCameraBounds
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("trigger2d with player");
        PlayerMovementStateMachine player = collision.gameObject.GetComponent<PlayerMovementStateMachine>();

        if (player != null)
        {
            _followCamera.SetNewCameraBounds(this);
        }    
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerMovementStateMachine player = collision.gameObject.GetComponent<PlayerMovementStateMachine>();

        if (player != null)
        {
            _followCamera.SetNewCameraBounds(null);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider2D>();
        _followCamera = FindObjectOfType<FollowCamera>();
    }

    public override float GetTargetXOffset()
    {
        return 0.0f;
    }

    public override float GetTargetYOffset()
    {
        return 0.0f;
    }
}
