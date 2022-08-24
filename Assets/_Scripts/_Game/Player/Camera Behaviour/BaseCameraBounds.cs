using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum CameraBoundsDirection
{
    All,
    Up,
    Down,
    Left,
    Right,
}

public abstract class BaseCameraBounds : MonoBehaviour
{
    [SerializeField]
    private bool[] _directions = new bool[4] { false, false, false, false };

    [SerializeField]
    private float _lerpXSpeed = 5.0f;
    [SerializeField]
    private float _lerpYSpeed = 5.0f;

    protected BoxCollider _boxCollider;

    protected FollowCamera _followCamera;

    public bool[] Directions { get => _directions; }
    public float LerpXSpeed { get => _lerpXSpeed; set => _lerpXSpeed = value; }
    public float LerpYSpeed { get => _lerpYSpeed; set => _lerpYSpeed = value; }
    
    public BoxCollider BoxCollider { get => _boxCollider; }

    public abstract float GetTargetXOffset();
    public abstract float GetTargetYOffset();
}
