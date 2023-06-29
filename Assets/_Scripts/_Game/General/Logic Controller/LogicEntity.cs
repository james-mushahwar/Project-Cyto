using _Scripts._Game.General.SaveLoad;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Scripts._Game.General.LogicController{

    [RequireComponent(typeof(SaveableEntity))]
    public class LogicEntity : MonoBehaviour, ILogicEntity, ISaveable
    {
        //IlogicEntity
        [SerializeField]
        private ELogicType _logicType;
        private bool _isInputLogicValid;
        private bool _isOutputLogicValid;
        [SerializeField]
        private List<LogicEntity> _inputs;
        [SerializeField]
        private List<LogicEntity> _outputs;
        private UnityEvent _onInputChanged = new UnityEvent();
        private UnityEvent _onOutputChanged = new UnityEvent();

        public ELogicType LogicType
        {
            get { return _logicType; }
        }
        public bool IsInputLogicValid
        {
            get { return _isInputLogicValid; }
            set { _isInputLogicValid = value; }
        }
        public bool IsOutputLogicValid
        {
            get { return _isOutputLogicValid; }
            set { _isOutputLogicValid = value; }
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
                isInputLogicValid = _isInputLogicValid,
                isOutputLogicValid = _isOutputLogicValid
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
