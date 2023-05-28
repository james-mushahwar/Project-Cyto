using _Scripts._Game.AI.Bonding;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General{

    // GAMEPLAY //////////////////////////////////////
    #region Combat
    public interface IDamageable
    {
        IExposable Exposable { get; }
        bool IsAlive();
        void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition);
        Vector2 DamageDirection { get; set; }
        bool FacingRight { get; }
        //Components
        Transform Transform { get; }
        //Inputs
        Vector2 GetMovementInput();
    }

    public interface IExposable
    {
        bool IsExposed();
        void OnExposed();
        void OnUnexposed();
    }
    #endregion

    #region Bonding
    public enum PossessInput
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
        COUNT
    }
    public interface IPossessControllable
    {
        #region Inputs
        public Dictionary<PossessInput, Action<InputAction.CallbackContext>> BondInputsDict { get; }

        // sticks
        public void OnMovementInput(InputAction.CallbackContext context); // left stick
        public void OnDirectionInput(InputAction.CallbackContext context); // right stick

        // buttons
        public void OnNorthButtonInput(InputAction.CallbackContext context); // Y button
        public void OnSouthButtonInput(InputAction.CallbackContext context); // A button
        public void OnEastButtonInput(InputAction.CallbackContext context); // B button
        public void OnWestButtonInput(InputAction.CallbackContext context); // X button

        // bumpers and triggers
        public void OnLeftBumperInput(InputAction.CallbackContext context); // LB button
        public void OnRightBumperInput(InputAction.CallbackContext context); // RB button
        public void OnLeftTriggerInput(InputAction.CallbackContext context); // LT button
        public void OnRightTriggerInput(InputAction.CallbackContext context); // RT button
        #endregion

        #region Bonding
        public void OnPossess();
        public void OnDispossess();
        #endregion
    }

    public enum EBondBehaviourType
    {
        Possess,
        Bounce,
        Phase,
        NONE
    }

    public interface IBondable
    {
        EBondBehaviourType BondBehaviourType { get; }
        Transform BondTargetTransform { get; }
        float SqDistanceToCompleteBond { get; }

        void OnStartBond();

        bool CanBeBonded();

        IPossessable Possessable { get; }
        IBounceable Bounceable { get; }
        IPhaseable Phaseable { get; }
        //BondBehaviour BondBehaviour { get; set; }
    }
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

    public interface IBounceable
    {

    }

    public interface IPhaseable
    {

    }

    #endregion

    #region Targeting
    public interface ITarget
    {
        public ETargetType TargetType { get; }
        public Transform GetTargetTransform();
        public void ManagedTargetTick();
    }
    #endregion

    // SYSTEMS //////////////////////////////////////
    #region RuntimeID
    public interface IRuntimeId
    {
        public RuntimeID RuntimeID { get; }
    }
    #endregion

    #region TickGroup
    public interface ITickMaster
    {
        bool IsUsingTickMaster();
        Int16 GetTickID();
    }

    public interface ITickGroup
    {
        public UniqueTickGroup UniqueTickGroup { get; }
    }
    #endregion

    #region UI

    public enum UIInputState
    {
        None,
        // everything in pause menu
        PauseMenu,
        Options,
        Save,
        //everything in inventory
        Inventory,
        //everything else
        Vendor,
        COUNT,
    }

    public enum EPlayerInput
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
        COUNT
    }
    #endregion


    #region Saving
    public interface ISaveable
    {
        object SaveState();

        void LoadState(object state);

    }
    #endregion
}
