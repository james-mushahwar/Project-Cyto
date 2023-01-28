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
        AIType _chosenAI = AIType.BombDroid;

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

        #endregion

        #region Movement
        bool[] _movementStates = new bool[6] { true, true, true, true, true, true };
        bool _movementGroupEnabled = true;
        bool[] _bondedMovementStates = new bool[6] { true, true, true, true, true, true };
        bool _bondedMovementGroupEnabled = true;
        #endregion

        #region Attack
        string[] _attackStates = new string[3];
        bool _attackGroupEnabled = true;
        string[] _bondedAttackStates = new string[3];
        bool _bondedAttackGroupEnabled = true;
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
            _chosenAI = (AIType) EditorGUILayout.EnumPopup("Chosen AI:", _chosenAI);

            EditorGUILayout.LabelField("Movement State Path:", _movementStatePaths[(int)_chosenAI]);
            EditorGUILayout.LabelField("Attack State Path:", _attackStatePaths[(int)_chosenAI]);

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
            _attackStates[0] = EditorGUILayout.TextField("Attack 1", _attackStates[0]);
            _attackStates[1] = EditorGUILayout.TextField("Attack 2", _attackStates[1]);
            _attackStates[2] = EditorGUILayout.TextField("Attack 3", _attackStates[2]);
            EditorGUILayout.EndToggleGroup();

            GUILayout.Space(20);
            #endregion

            #region Animator
            GUILayout.Label("Animator", EditorStyles.boldLabel);
            GUILayout.Space(20);
            #endregion


            if (GUILayout.Button("Generate states!"))
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
            }

            #region Movement

            string newGenScriptName = "";
            if (_movementGroupEnabled)
            {

                string scriptType = "AIMovementState.cs.txt";
                string scriptBaseType = "AIMovementState";

                if (_movementStates[0])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Sleep", scriptBaseType);
                }
                if (_movementStates[1])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Wake", scriptBaseType);
                }
                if (_movementStates[2])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Idle", scriptBaseType);
                }
                if (_movementStates[3])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Patrol", scriptBaseType);
                }
                if (_movementStates[4])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Chase", scriptBaseType);
                }
                if (_movementStates[5])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Attack", scriptBaseType);
                }
            }
            if (_bondedMovementGroupEnabled)
            {

                string scriptType = "AIBondedMovementState.cs.txt";
                string scriptBaseType = "AIBondedMovementState";

                if (_bondedMovementStates[0])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Grounded", scriptBaseType);
                }
                if (_bondedMovementStates[1])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Jumping", scriptBaseType);
                }
                if (_bondedMovementStates[2])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Falling", scriptBaseType);
                }
                if (_bondedMovementStates[3])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Flying", scriptBaseType);
                }
                if (_bondedMovementStates[4])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Dashing", scriptBaseType);
                }
                if (_bondedMovementStates[5])
                {
                    CreateNewCSScript(_movementStatePaths[(int)_chosenAI], scriptType, "Attacking", scriptBaseType);
                }
            }

            #endregion

            #region Attack States

            if (_attackGroupEnabled)
            {
                string scriptType = "AIAttackState.cs.txt";
                string scriptBaseType = "AIAttackState";

                CreateNewCSScript(_attackStatePaths[(int)_chosenAI], scriptType, "Idle", scriptBaseType);
                if (_attackStates[0] != "")
                {
                    CreateNewCSScript(_attackStatePaths[(int)_chosenAI], scriptType, _attackStates[0], scriptBaseType);
                    _attackStates[0] = "";
                }
                if (_attackStates[1] != "")
                {
                    CreateNewCSScript(_attackStatePaths[(int)_chosenAI], scriptType, _attackStates[1], scriptBaseType);
                    _attackStates[1] = "";
                }
                if (_attackStates[2] != "")
                {
                    CreateNewCSScript(_attackStatePaths[(int)_chosenAI], scriptType, _attackStates[2], scriptBaseType);
                    _attackStates[2] = "";
                }
            }

            #endregion

            void CreateNewCSScript(string pathName, string templateType, string actionType, string baseType)
            {
                newGenScriptName = _namePrefixes[(int)_chosenAI] + actionType + baseType + ".cs";
                if (!System.IO.File.Exists(pathName + "/" + newGenScriptName))
                {
                    ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_aiTemplatePath + templateType, pathName + "/" + newGenScriptName);
                    Debug.Log("Created C# script: " + newGenScriptName);
                }
            }
        }
    }
    
}
