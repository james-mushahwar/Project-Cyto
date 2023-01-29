using UnityEngine;
using UnityEditor;
using System.IO;

namespace _Scripts.Editortools.Windows.AI{
    
    public enum AIType
    {
        BombDroid,
        MushroomArcher,
        DaggerMushroom,
        COUNT
    }

    public class AIGenerator : EditorWindow
    {
        #region General
        private const string _aiTemplatePath = "Assets/_Scripts/Editor tools/Templates/AI/";

        private Vector2 _scrollPos = Vector2.zero;
        #endregion

        #region Chosen AI
        AIType _chosenAI = AIType.COUNT;
        bool _selectAll = false;

        string[] _namePrefixes = new string[(int)AIType.COUNT]
        {
            "BombDroid",        // Bomb Droid
            "MushroomArcher",   // Mushroom Archer
            "DaggerMushroom",   // Dagger Mushroom
        };

        string[] _movementStatePaths = new string[(int)AIType.COUNT]
        {
            "Assets/_Scripts/_Game/AI/Movement State Machine/Flying/Bomb droid",        // Bomb Droid
            "Assets/_Scripts/_Game/AI/Movement State Machine/Ground/Mushroom Archer",   // Mushroom Archer
            "Assets/_Scripts/_Game/AI/Movement State Machine/Ground/Dagger Mushroom",   // Dagger Mushroom
        };

        string[] _attackStatePaths = new string[(int)AIType.COUNT]
        {
            "Assets/_Scripts/_Game/AI/Attack State Machine/Flying/Bomb droid",      // Bomb Droid
            "Assets/_Scripts/_Game/AI/Attack State Machine/Ground/Mushroom Archer", // Mushroom Archer
            "Assets/_Scripts/_Game/AI/Attack State Machine/Ground/Dagger Mushroom", // Dagger Mushroom
        };

        string[] _animatorPaths = new string[(int)AIType.COUNT]
        {
            "Assets/_Scripts/_Game/Animation/Character/AI/Flying/Bomb Droid",      // Bomb Droid
            "Assets/_Scripts/_Game/Animation/Character/AI/Ground/Mushroom Archer", // Mushroom Archer
            "Assets/_Scripts/_Game/Animation/Character/AI/Ground/Dagger Mushroom", // Dagger Mushroom

        };

        #endregion

        #region Movement
        bool[] _movementStates = new bool[6] { false, false, false, false, false, false };
        bool _movementGroupEnabled = true;
        bool[] _bondedMovementStates = new bool[6] { false, false, false, false, false, false };
        bool _bondedMovementGroupEnabled = true;
        #endregion

        #region Attack
        bool _idleAttackState = false;
        string[] _attackStates = new string[3];
        bool _attackGroupEnabled = true;
        string[] _bondedAttackStates = new string[3];
        bool _bondedAttackGroupEnabled = true;
        #endregion

        #region Sprite Animator
        bool _spriteGroupEnabled = true;
        #endregion

        [MenuItem("Window/AI Generator")]
        public static void ShowWindow()
        {
            GetWindow<AIGenerator>("AI Generator");
        }

        private void OnGUI()
        {
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            #region General

            GUILayout.BeginHorizontal("box");
            _chosenAI = (AIType) EditorGUILayout.EnumPopup("Chosen AI:", _chosenAI);
            _selectAll = EditorGUILayout.Toggle("Select All AI", _selectAll);
            GUILayout.EndHorizontal();

            if (_chosenAI == AIType.COUNT)
            {
                EditorGUILayout.EndScrollView();
                return;
            }
            EditorGUILayout.LabelField("Movement State Path:", _movementStatePaths[(int)_chosenAI]);
            EditorGUILayout.LabelField("Attack State Path:", _attackStatePaths[(int)_chosenAI]);
            EditorGUILayout.LabelField("Sprite Animator Path:", _animatorPaths[(int)_chosenAI]);

            GUILayout.Space(20);
            #endregion

            #region Movement
            GUILayout.Label("Movement States", EditorStyles.boldLabel);
            _movementGroupEnabled = EditorGUILayout.BeginToggleGroup("Movement States", _movementGroupEnabled);
            _movementStates[0] = EditorGUILayout.Toggle("Sleep", _movementStates[0]);
            _movementStates[1] = EditorGUILayout.Toggle("Wake", _movementStates[1]);
            _movementStates[2] = EditorGUILayout.Toggle("Idle", _movementStates[2]);
            _movementStates[3] = EditorGUILayout.Toggle("Patrol", _movementStates[3]);
            _movementStates[4] = EditorGUILayout.Toggle("Chase", _movementStates[4]);
            _movementStates[5] = EditorGUILayout.Toggle("Attack", _movementStates[5]);
            EditorGUILayout.EndToggleGroup();

            _bondedMovementGroupEnabled = EditorGUILayout.BeginToggleGroup("Bonded Movement States", _bondedMovementGroupEnabled);
            _bondedMovementStates[0] = EditorGUILayout.Toggle("Grounded", _bondedMovementStates[0]);
            _bondedMovementStates[1] = EditorGUILayout.Toggle("Jumping", _bondedMovementStates[1]);
            _bondedMovementStates[2] = EditorGUILayout.Toggle("Falling", _bondedMovementStates[2]);
            _bondedMovementStates[3] = EditorGUILayout.Toggle("Flying", _bondedMovementStates[3]);
            _bondedMovementStates[4] = EditorGUILayout.Toggle("Dashing", _bondedMovementStates[4]);
            _bondedMovementStates[5] = EditorGUILayout.Toggle("Attacking", _bondedMovementStates[5]);
            EditorGUILayout.EndToggleGroup();

            GUILayout.Space(20);
            #endregion

            #region Attack
            GUILayout.Label("Attack States", EditorStyles.boldLabel);
            _attackGroupEnabled = EditorGUILayout.BeginToggleGroup("Attack States", _attackGroupEnabled);
            _idleAttackState = EditorGUILayout.Toggle("Idle", _idleAttackState);
            _attackStates[0] = EditorGUILayout.TextField("Attack 1", _attackStates[0]);
            _attackStates[1] = EditorGUILayout.TextField("Attack 2", _attackStates[1]);
            _attackStates[2] = EditorGUILayout.TextField("Attack 3", _attackStates[2]);
            EditorGUILayout.EndToggleGroup();

            GUILayout.Space(20);
            #endregion

            #region Animator
            GUILayout.Label("Animator", EditorStyles.boldLabel);
            _spriteGroupEnabled = EditorGUILayout.BeginToggleGroup("Sprite animator", _spriteGroupEnabled);
            EditorGUILayout.EndToggleGroup();
            GUILayout.Space(20);
            #endregion


            if (GUILayout.Button("Generate scripts!"))
            {
                Generate();
            }
            EditorGUILayout.EndScrollView();

            this.Repaint();
        }

        void Generate()
        {
            // make all dirctories first
            for (int i = 0; i < (int)AIType.COUNT; i++)
            {
                if (_movementStatePaths[i] != "" && !Directory.Exists(_movementStatePaths[i]))
                {
                    Debug.Log("Created Movement Directory: " + _movementStatePaths[i]);
                    Directory.CreateDirectory(_movementStatePaths[i]);
                }

                if (_attackStatePaths[i] != "" && !Directory.Exists(_attackStatePaths[i]))
                {
                    Debug.Log("Created Attack Directory: " + _attackStatePaths[i]);
                    Directory.CreateDirectory(_attackStatePaths[i]);
                }

                if (_animatorPaths[i] != "" && !Directory.Exists(_animatorPaths[i]))
                {
                    Debug.Log("Created Animator Directory: " + _animatorPaths[i]);
                    Directory.CreateDirectory(_animatorPaths[i]);
                }
            }

            if (_chosenAI == AIType.COUNT)
            {
                return;
            }

            // select one or select all AI types
            int index = _selectAll ? 0 : (int)_chosenAI;
            int lastIndex = _selectAll ? (int)AIType.COUNT : (int)_chosenAI + 1;

            for (int i = index; i < lastIndex; i++)
            {
                #region State Machines
                string stateType = "";
                string stateBaseType = "";

                //movement state machine
                stateType = _movementStatePaths[i].Contains("Flying") ? "FlyingAIMovementStateMachine.cs.txt" : "GroundAIMovementStateMachine.cs.txt";
                stateBaseType = "AIMovementStateMachine";
                CreateNewCSScript(i, _movementStatePaths[i], stateType, "", stateBaseType);

                //attacking state machine
                stateType = "AIAttackStateMachine.cs.txt";
                stateBaseType = "AIAttackStateMachineBase";
                CreateNewCSScript(i, _attackStatePaths[i], stateType, "", stateBaseType);
                #endregion

                #region Movement
                if (_movementGroupEnabled)
                {
                    string scriptType = "AIMovementState.cs.txt";
                    string scriptBaseType = "AIMovementState";

                    if (_movementStates[0])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Sleep", scriptBaseType);
                    }
                    if (_movementStates[1])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Wake", scriptBaseType);
                    }
                    if (_movementStates[2])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Idle", scriptBaseType);
                    }
                    if (_movementStates[3])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Patrol", scriptBaseType);
                    }
                    if (_movementStates[4])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Chase", scriptBaseType);
                    }
                    if (_movementStates[5])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Attack", scriptBaseType);
                    }
                }

                if (_bondedMovementGroupEnabled)
                {

                    string scriptType = "AIBondedMovementState.cs.txt";
                    string scriptBaseType = "AIBondedMovementState";

                    if (_bondedMovementStates[0])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Grounded", scriptBaseType);
                    }
                    if (_bondedMovementStates[1])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Jumping", scriptBaseType);
                    }
                    if (_bondedMovementStates[2])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Falling", scriptBaseType);
                    }
                    if (_bondedMovementStates[3])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Flying", scriptBaseType);
                    }
                    if (_bondedMovementStates[4])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Dashing", scriptBaseType);
                    }
                    if (_bondedMovementStates[5])
                    {
                        CreateNewCSScript(i, _movementStatePaths[i], scriptType, "Attacking", scriptBaseType);
                    }
                }
                #endregion

                #region Attack States
                if (_attackGroupEnabled)
                {
                    string scriptType = "AIAttackState.cs.txt";
                    string scriptBaseType = "AIAttackState";

                    CreateNewCSScript(i,_attackStatePaths[i], scriptType, "Idle", scriptBaseType);
                    if (_attackStates[0] != "")
                    {
                        CreateNewCSScript(i, _attackStatePaths[i], scriptType, _attackStates[0], scriptBaseType);
                        _attackStates[0] = "";
                    }
                    if (_attackStates[1] != "")
                    {
                        CreateNewCSScript(i, _attackStatePaths[i], scriptType, _attackStates[1], scriptBaseType);
                        _attackStates[1] = "";
                    }
                    if (_attackStates[2] != "")
                    {
                        CreateNewCSScript(i, _attackStatePaths[i], scriptType, _attackStates[2], scriptBaseType);
                        _attackStates[2] = "";
                    }
                }
                #endregion


                #region Sprite Animation
                if (_spriteGroupEnabled)
                {
                    string scriptType = "SpriteAnimator.cs.txt";
                    string scriptBaseType = "SpriteAnimator";

                    CreateNewCSScript(i, _animatorPaths[i], scriptType, "", scriptBaseType);
                }
                #endregion

                _selectAll = false;
            }

            void CreateNewCSScript(int index, string pathName, string templateType, string actionType, string baseType)
            {
                string newGenScriptName = _namePrefixes[index] + actionType + baseType + ".cs";
                Debug.Log(pathName + "/" + newGenScriptName);
                if (!System.IO.File.Exists(pathName + "/" + newGenScriptName))
                {
                    ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_aiTemplatePath + templateType, pathName + "/" + newGenScriptName);
                    Debug.Log("Created C# script: " + newGenScriptName);
                }
                else
                {
                    Debug.Log("Not generating duplicate file: " + pathName + "/" + newGenScriptName);
                }
            }
        }
    }
    
}
