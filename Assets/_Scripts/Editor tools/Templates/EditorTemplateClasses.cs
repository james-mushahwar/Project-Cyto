#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Editor.Templates{
    
    public class EditorTemplateClasses
    {
        #region AI
        private const string _aiPath = "Assets/_Scripts/Editor tools/Templates/AI/";
        private const string _entityPath = "Assets/_Scripts/Editor tools/Templates/AI/Entity";
        private const string _movementPath = "Assets/_Scripts/Editor tools/Templates/AI/Movement";
        private const string _attackPath = "Assets/_Scripts/Editor tools/Templates/AI/Attack";
        private const string _animPath = "Assets/_Scripts/Editor tools/Templates/AI/Animation";


        private const string _aiMovementState = "AIMovementState.cs.txt";

        [MenuItem(itemName: "Assets/Create/Scripts/AI/AI Movement State", isValidateFunction: false, priority: 51)]
        public static void CreateAIMovementStateFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_aiPath + _aiMovementState, "AIMovementState.cs");
        }
        
        private const string _aiBondedMovementState = "AIBondedMovementState.cs.txt";

        [MenuItem(itemName: "Assets/Create/Scripts/AI/AI Bonded Movement State", isValidateFunction: false, priority: 51)]
        public static void CreateAIBondedMovementStateFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_aiPath + _aiBondedMovementState, "AIBondedMovementState.cs");
        }

        private const string _aiAttackState = "AIAttackState.cs.txt";

        [MenuItem(itemName: "Assets/Create/Scripts/AI/AI Attack State", isValidateFunction: false, priority: 51)]
        public static void CreateAIAttackStateFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_aiPath + _aiAttackState, "AIAttackState.cs");
        }
        #endregion


        #region General
        private const string _generalPath = "Assets/_Scripts/Editor tools/Templates/General/";
        private const string _emptyClassTemplate = "EmptyClass.cs.txt";

        [MenuItem(itemName: "Assets/Create/Scripts/General/C# Class", isValidateFunction: false, priority: 51)]
        public static void CreateClassFromTemplate()
        {
            ProjectWindowUtil.CreateScriptAssetFromTemplateFile(_generalPath + _emptyClassTemplate, "EmptyClass.cs");
        }
        #endregion

    }

}
#endif