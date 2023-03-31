using _Scripts._Game.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public interface IPossessable
    {
        bool IsPossessed();
        bool CanBePossessed();
        void OnPossess();
        void OnDispossess();
        bool FacingRight { get; }
        //Components
        Transform Transform { get; }

        //Inputs
        Vector2 GetMovementInput();
    }
}
