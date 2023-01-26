using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace _Scripts.Editortools.Windows.AI{
    
    public enum AIType
    {
        BombDroid,
        MushroomArcher,
        DaggerMushroom,
    }

    public class AIGenerator : EditorWindow
    {
        [MenuItem("Window/AI Generator")]
        public static void ShowWindow()
        {
            GetWindow<AIGenerator>("AI Generator");
        }



        #region Chosen AI
        AIType _chosenAI = AIType.BombDroid;

        string[] _movementStatePaths = new string[3]
        {
            "Assets/_Scripts/_Game/AI/Movement State Machine/Flying/Bomb droid", // Bomb Droid
            "Assets/_Scripts/_Game/AI/Movement State Machine/Ground/Mushroom Archer", // Mushroom Archer
            "Assets/_Scripts/_Game/AI/Movement State Machine/Ground/Dagger Mushroom", // Dagger Mushroom
        };

        string[] _attackStatePaths = new string[3]
        {
            "Assets/_Scripts/_Game/AI/Attack State Machine/Flying/Bomb droid", // Bomb Droid
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

        private void OnGUI()
        {
            #region General
            _chosenAI = (AIType) EditorGUILayout.EnumPopup("Chosen AI:", _chosenAI);

            EditorGUILayout.LabelField("Movement State Path:", _movementStatePaths[(int)_chosenAI]);
            EditorGUILayout.LabelField("Attack State Path:", _attackStatePaths[(int)_chosenAI]);

            GUILayout.Space(20);
            #endregion

            #region Movement
            GUILayout.Label("Movement States", EditorStyles.boldLabel);
            _movementGroupEnabled = EditorGUILayout.BeginToggleGroup("MovementStates", _movementGroupEnabled);
            _movementStates[0] = EditorGUILayout.Toggle("Sleep", _movementStates[0]);
            _movementStates[1] = EditorGUILayout.Toggle("Wake", _movementStates[1]);
            _movementStates[2] = EditorGUILayout.Toggle("Idle", _movementStates[2]);
            _movementStates[3] = EditorGUILayout.Toggle("Patrol", _movementStates[3]);
            _movementStates[4] = EditorGUILayout.Toggle("Chase", _movementStates[4]);
            _movementStates[5] = EditorGUILayout.Toggle("Attack", _movementStates[5]);
            EditorGUILayout.EndToggleGroup();

            _bondedMovementGroupEnabled = EditorGUILayout.BeginToggleGroup("BondedMovementStates", _bondedMovementGroupEnabled);
            _bondedMovementStates[0] = EditorGUILayout.Toggle("Grounded", _bondedMovementStates[0]);
            _bondedMovementStates[1] = EditorGUILayout.Toggle("Jumping", _bondedMovementStates[1]);
            _bondedMovementStates[2] = EditorGUILayout.Toggle("Falling", _bondedMovementStates[2]);
            _bondedMovementStates[3] = EditorGUILayout.Toggle("Flying", _bondedMovementStates[3]);
            _bondedMovementStates[4] = EditorGUILayout.Toggle("Dashing", _bondedMovementStates[4]);
            _bondedMovementStates[5] = EditorGUILayout.Toggle("Attacking", _bondedMovementStates[5]);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button("Generate states!"))
                Generate();

            GUILayout.Space(20);
            #endregion


            GUILayout.Label("Attack States", EditorStyles.boldLabel);

            GUILayout.Label("Animator", EditorStyles.boldLabel);

            this.Repaint();
        }

        void Generate()
        {
            #region Movement
            //FileInfo file = new FileInfo(_movementStatePaths[(int)_chosenAI]);
            //file.Directory.Create(); // If the directory already exists, this method does nothing.

            if (!Directory.Exists(_movementStatePaths[(int)_chosenAI]))
            {
                Directory.CreateDirectory(_movementStatePaths[(int)_chosenAI]);
            }
            #endregion
        }
    }
    
}
