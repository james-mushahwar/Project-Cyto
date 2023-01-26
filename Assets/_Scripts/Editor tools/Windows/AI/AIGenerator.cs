using UnityEngine;
using UnityEditor;

namespace _Scripts.Editortools.Windows.AI{
    
    public enum AIType
    {
        BombDroid,
        ArcherDroid
    }

    public class AIGenerator : EditorWindow
    {
        [MenuItem("Window/AI Generator")]
        public static void ShowWindow()
        {
            GetWindow<AIGenerator>("AI Generator");
        }



        #region Target AI
        

        string[] MovementStatePaths = new string[1]
        {
            "",
        };

        #endregion

        #region Movement
        [SerializeField]
        bool[] movementStates = new bool[6] { true, true, true, true, true, true };
        bool movementGroupEnabled = true;
        bool[] bondedMovementStates = new bool[6] { true, true, true, true, true, true };
        bool bondedMovementGroupEnabled = true;
        #endregion

        private void OnGUI()
        {
            #region Movement
            GUILayout.Label("Movement States", EditorStyles.boldLabel);
            movementGroupEnabled = EditorGUILayout.BeginToggleGroup("MovementStates", movementGroupEnabled);
            movementStates[0] = EditorGUILayout.Toggle("Sleep", movementStates[0]);
            movementStates[1] = EditorGUILayout.Toggle("Wake", movementStates[1]);
            movementStates[2] = EditorGUILayout.Toggle("Idle", movementStates[2]);
            movementStates[3] = EditorGUILayout.Toggle("Patrol", movementStates[3]);
            movementStates[4] = EditorGUILayout.Toggle("Chase", movementStates[4]);
            movementStates[5] = EditorGUILayout.Toggle("Attack", movementStates[5]);
            EditorGUILayout.EndToggleGroup();

            bondedMovementGroupEnabled = EditorGUILayout.BeginToggleGroup("BondedMovementStates", bondedMovementGroupEnabled);
            bondedMovementStates[0] = EditorGUILayout.Toggle("Grounded", bondedMovementStates[0]);
            bondedMovementStates[1] = EditorGUILayout.Toggle("Jumping", bondedMovementStates[1]);
            bondedMovementStates[2] = EditorGUILayout.Toggle("Falling", bondedMovementStates[2]);
            bondedMovementStates[3] = EditorGUILayout.Toggle("Flying", bondedMovementStates[3]);
            bondedMovementStates[4] = EditorGUILayout.Toggle("Dashing", bondedMovementStates[4]);
            bondedMovementStates[5] = EditorGUILayout.Toggle("Attacking", bondedMovementStates[5]);
            EditorGUILayout.EndToggleGroup();

            if (GUILayout.Button("Generate states!"))
                Generate();

            GUILayout.Space(20);
            #endregion


            GUILayout.Label("Attack States", EditorStyles.boldLabel);

            GUILayout.Label("Animator", EditorStyles.boldLabel);

        }

        void Generate()
        {

        }
    }
    
}
