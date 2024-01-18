using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.LogicController{

    [RequireComponent(typeof(SaveableEntity))]
    public class LogicEntity : MonoBehaviour, ILogicEntity, ISaveable
    {
        //IlogicEntity
        [Header("Input logic")]
        [SerializeField]
        private ELogicType _inputLogicType;
        [SerializeField]
        private ELogicConditionType _inputConditionType;
        private bool _isInputLogicValid;
        [SerializeField]
        private List<LogicEntity> _inputs;
        private UnityEvent _onInputChanged = new UnityEvent();

        [Space]

        [Header("Output logic")]
        [SerializeField]
        private ELogicType _outputLogicType;

        [HeaderAttribute("Separate Output")]
        [SerializeField]
        private bool _useSeparateOutputLogic = true;
        [SerializeField]
        private bool _isOutputLogicValid;

        [Space]
        [SerializeField]
        private List<LogicEntity> _outputs;
        private UnityEvent _onOutputChanged = new UnityEvent();

        public ELogicType InputLogicType
        {
            get { return _inputLogicType; }
        }

        public ELogicConditionType InputConditionType
        {
            get { return _inputConditionType; }
        }

        public ELogicType OutputLogicType
        {
            get { return _outputLogicType; }
        }

        public bool UseSeparateOutputLogic
        {
            get { return _useSeparateOutputLogic; }
        }

        public bool IsInputLogicValid
        {
            get { return _isInputLogicValid; }
            set { _isInputLogicValid = value; }
        }
        public bool IsOutputLogicValid
        {
            get
            {
                if (_useSeparateOutputLogic)
                {
                    return _isOutputLogicValid;
                }
                else
                {
                    return LogicManager.Instance.AreAllInputsValid(this);
                }
            }
            set
            {
                if (_useSeparateOutputLogic)
                {
                    _isOutputLogicValid = value;
                }
            }
        }

        public UnityEvent OnInputChanged
        {
            get { return _onInputChanged; }
        }

        public UnityEvent OnOutputChanged
        {
            get { return _onOutputChanged; }
        }

        public List<LogicEntity> Inputs
        {
            get { return _inputs; }
        }
        public List<LogicEntity> Outputs
        {
            get { return _outputs; }
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }
    
        // Update is called once per frame
        void Update()
        {
            
        }

        //ISaveable
        [System.Serializable]
        private struct SaveData
        {
            public bool isInputLogicValid;
            public bool isOutputLogicValid;
        }

        public object SaveState()
        {
            return new SaveData()
            {
                isInputLogicValid = IsInputLogicValid,
                isOutputLogicValid = IsOutputLogicValid
            };
        }

        public void LoadState(object state)
        {
            SaveData saveData = (SaveData)state;

            _isInputLogicValid = saveData.isInputLogicValid;
            _isOutputLogicValid = saveData.isOutputLogicValid;
        }

    }
    
}
