using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class LogicManager : Singleton<LogicManager>, IManager
    {
        public bool AreAllInputsValid(ILogicEntity logicEntity)
        {
            foreach (LogicEntity input in logicEntity.Inputs)
            {
                if (!input.IsOutputLogicValid)
                {
                    return false;
                }

                if (!AreAllInputsValid(input))
                {
                    return false;
                }
            }

            return true;
        }

        public void OnOutputChanged(ILogicEntity logicEntity)
        {
            bool newOutput = logicEntity.IsOutputLogicValid;
            foreach (LogicEntity output in logicEntity.Outputs)
            {
                if (newOutput)
                {
                    if (AreAllInputsValid(output) != output.IsInputLogicValid)
                    {
                        output.IsInputLogicValid = !output.IsInputLogicValid;
                        output.OnInputChanged.Invoke();
                        //OnOutputChanged(output);
                    }
                }
            }
        }

        public void PreInGameLoad()
        {
             
        }

        public void PostInGameLoad()
        {
             
        }

        public void PreMainMenuLoad()
        {
             
        }

        public void PostMainMenuLoad()
        {
             
        }
    }
    
}
