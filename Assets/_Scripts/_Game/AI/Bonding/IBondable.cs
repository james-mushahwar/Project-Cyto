using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Bonding{
    
    public interface IBondable
    {
        void OnMovementInput(InputAction.CallbackContext context); // left stick
        void OnDirectionInput(InputAction.CallbackContext context); // right stick
        void OnJumpInput(InputAction.CallbackContext context); // A button
        void OnDashInput(InputAction.CallbackContext context); // RB button
        void OnFloatInput(InputAction.CallbackContext context); // RT trigger
        void OnBounceInput(InputAction.CallbackContext context); // LT trigger
    }
    
}
