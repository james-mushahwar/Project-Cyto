using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    private Dictionary<CameraBoundsDirection, bool> _cameraBoundsDict = new Dictionary<CameraBoundsDirection, bool>();

    protected FollowCamera _followCamera;

    public Dictionary<CameraBoundsDirection, bool> CameraBoundsDict { get => _cameraBoundsDict; }

    public abstract float GetTargetXOffset();
    public abstract float GetTargetYOffset();
}
