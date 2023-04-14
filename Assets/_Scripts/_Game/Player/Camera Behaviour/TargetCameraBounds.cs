using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetCameraBounds : BaseCameraBounds
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

    private void Awake()
    {
    }

    private void OnEnable()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        _followCamera = FindObjectOfType<FollowCamera>();
    }

    public override float GetTargetXOffset()
    {
        throw new System.NotImplementedException();
    }

    public override float GetTargetYOffset()
    {
        throw new System.NotImplementedException();
    }
}
