using _Scripts._Game.AI.Bonding;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using _Scripts._Game.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Scripts._Game.General{

    // GAMEPLAY //////////////////////////////////////
    #region Combat
    public enum EHealthStatType
    {
        Player,
        EnemyHealth,
        BondableHealth
    }

    public enum EEntityType
    {
        Player,
        Ally,
        Enemy,
        BondedEnemy
    }

    public enum EDamageType
    {
        Player_BasicAttack,
        BombDroid_BombDrop_DirectHit,
        BombDroid_BombDrop_Explosion,
    }

    public interface IDamageable
    {
        IExposable Exposable { get; }
        bool IsAlive();
        void TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition);
        Vector2 DamageDirection { get; set; }
        //Components
        Transform Transform { get; }
    }

    public interface IDamageCauser
    {
        //Inputs
        Transform Transform { get; }
        Vector2 GetMovementInput();
        bool FacingRight { get; }
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

    #region Interactable
    [System.Serializable]
    public struct RangeParams
    {
        public float _maxSqDistance;
        public bool _useDotProduct;
        public Vector2 _dotRange;
        public Vector2 _dotDirection;

        public RangeParams(bool useDotProduct = false)
        {
            _maxSqDistance = 10.0f;
            _useDotProduct = useDotProduct;
            _dotRange = new Vector2(0.0f, 0.0f);
            _dotDirection = new Vector2(1.0f,1.0f);
        }
    }

    public interface IInteractable
    {
        public EInteractableStimuli InteractableStimuli { get; }
        public EInteractableType InteractableType { get; }
        public Transform InteractRoot { get; set; }
        public bool IsInteractionLocked { get; set; } // is interacting with or has already interacted
        public RangeParams RangeParams { get; }
        UnityEvent OnHighlight { get; }
        UnityEvent OnUnhighlight { get; }
        UnityEvent OnInteractStart { get; }
        UnityEvent OnInteractEnd { get; }
        public bool IsInteractable(); // do overlap or distance check

        public void OnInteract();
    }

    public enum EInteractableType
    {
        None,
        SaveStation,
        Doorway,
        COUNT,
    }

    public enum EInteractableStimuli
    {
        PlayerInput,
        CollisionInput
    }

    //public interface IReactable
    //{
    //    public bool CanReact { get; }
    //    public void OnStartReact();
    //    public void OnEndReact();
    //}
    #endregion

    #region Stats

    public enum EStatsType
    {
        // General
        TimePlayed,

        //World
        FriendsFound,
        HostilesFound,
        AreasFound,

        // Player actions
        Bonds,
        Possessions,
        Bounces,
        Phases,
        Exposures,

        COUNT
    }
    #endregion

    #region Entities
    public enum EEntity
    {
        Player,
        BombDroid,
        MushroomArcher,
        DaggerMushroom,
        COUNT
    }
    #endregion

    #region Logic Control

    public enum ELogicType
    {
        Constant = 0, // once on it stays on regardless of inputs before
        Dependent = 1, // dependent on all inputs being true
        Timed = 2, // once on it stays on for a limited time until switching off
        //Toggle = 3, // doesn't matter what input is but will flip validity if any input changes
    }

    public enum ELogicConditionType // what inputs does it take to make this input valid
    {
        All = 0,  // all inputs must be on to be valid (AND)
        Any = 1,  // at least 1 input must be on to be valid (OR)
        None = 2, // all inputs are off to be valid (NOT)
    }

    public interface ILogicEntity
    {
        public ELogicType InputLogicType { get; }
        public ELogicConditionType InputConditionType { get; }
        public bool IsInputLogicValid { get; set; } // are all inputs leading to this true?
        UnityEvent OnInputChanged { get; }

        public ELogicType OutputLogicType { get; }
        public bool UseSeparateOutputLogic { get; } // true use IsOutputLogicValid, false check all inputs are valid
        public bool IsOutputLogicValid { get; set; } // is this output true?
        UnityEvent OnOutputChanged { get; }

        public List<LogicEntity> Inputs { get; }
        public List<LogicEntity> Outputs { get; }
    }

    #endregion

    #region UI

    public enum UIInputState
    {
        None,
        // everything in main menu
        MainMenu,
        SaveFiles,
        // everything in pause menu
        PauseMenu,
        Options,
        //everything in inventory
        Inventory,
        //everything else
        Vendor,
        //loading screens
        LoadingScreen,
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

    #region GameState
    public enum EGameType
    {
        MainMenu,
        InGame,
    }

    public enum ELoadType
    {
        NONE,
        NewGame,
        LoadSave,
        LoadNewZone,
    }
    #endregion

    #region Saving
    [Serializable]
    public enum ESaveType
    {
        NONE,
        Manual,
        WorldEvent,
        COUNT,
    }

    [Serializable]
    public enum ESaveTarget
    {
        Saveable,
        GamePrefs,
        COUNT
    }

    [Serializable]
    public enum ELoadSpecifier
    {
        PlayTime,
        Location
    }

    public interface ISaveable
    {
        object SaveState();

        void LoadState(object state);
    }

    public interface IPrefsSaveable
    {
        object SavePrefs();

        void LoadPrefs(object state);
    }
    #endregion

    #region Managers

    public interface IManager
    {
        // managed tick
        public void ManagedTick()
        {

        }
        // before game starts loading
        public void PreInGameLoad();
        // just as game finishes loading
        public void PostInGameLoad();
        // before game starts loading
        public void PreMainMenuLoad();
        // just as game finishes loading
        public void PostMainMenuLoad();
    }

    public interface IManagedPool
    {
        public void ManagedTick()
        {

        }
    }
    #endregion
}
