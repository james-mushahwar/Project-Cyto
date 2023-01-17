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
        HealthStats GetHealthStats();
        //Inputs
        Vector2 GetMovementInput();
    }
}
