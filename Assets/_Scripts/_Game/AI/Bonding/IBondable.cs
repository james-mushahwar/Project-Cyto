using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Scripts._Game.AI.Bonding{
    
    public enum BondInput
    {
        Movement,
        Direction,
        NButton,
        SButton,
        EButton,
        WButton,
        LBumper,
        RBumper,
        LTrigger,
        RTrigger,
    }

    public interface IBondable
    {
        #region Inputs
        Dictionary<BondInput, Action> BondInputsDict { get; }

        // sticks
        void OnMovementInput(InputAction.CallbackContext context); // left stick
        void OnDirectionInput(InputAction.CallbackContext context); // right stick

        // buttons
        void OnNorthButtonInput(InputAction.CallbackContext context); // Y button
        void OnSouthButtonInput(InputAction.CallbackContext context); // A button
        void OnEastButtonInput(InputAction.CallbackContext context); // B button
        void OnWestButtonInput(InputAction.CallbackContext context); // X button

        // bumpers and triggers
        void OnLeftBumperInput(InputAction.CallbackContext context); // LB button
        void OnRightBumperInput(InputAction.CallbackContext context); // RB button
        void OnLeftTriggerInput(InputAction.CallbackContext context); // LT button
        void OnRightTriggerInput(InputAction.CallbackContext context); // LT button
        #endregion
    }

}
