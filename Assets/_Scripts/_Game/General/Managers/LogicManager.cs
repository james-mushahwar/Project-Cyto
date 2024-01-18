using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class LogicManager : Singleton<LogicManager>, IManager
    {
        public bool AreAllInputsValid(ILogicEntity logicEntity)
        {
            bool inputIsValid = true;

            // constant 
            if (logicEntity.InputLogicType == ELogicType.Constant)
            {
                if (logicEntity.IsInputLogicValid == true)
                {
                    return inputIsValid;
                }
            }

            if (logicEntity.InputConditionType == ELogicConditionType.All)
            {
                foreach (LogicEntity input in logicEntity.Inputs)
                {
                    if (!input.IsOutputLogicValid)
                    {
                        inputIsValid = false;
                        break;
                    }
                    
                    //if (!AreAllInputsValid(input))
                    //{
                    //    return false;
                    //}
                }
            }
            else if (logicEntity.InputConditionType == ELogicConditionType.Any)
            {
                inputIsValid = false;
                foreach (LogicEntity input in logicEntity.Inputs)
                {
                    if (input.IsOutputLogicValid)
                    {
                        inputIsValid = true;
                        break;
                    }
                }
            }
            else if (logicEntity.InputConditionType == ELogicConditionType.None)
            {
                foreach (LogicEntity input in logicEntity.Inputs)
                {
                    if (input.IsOutputLogicValid)
                    {
                        inputIsValid = false;
                        break;
                    }
                }
            }

            return inputIsValid;
        }

        public void OnOutputChanged(ILogicEntity logicEntity)
        {
            bool newOutput = logicEntity.IsOutputLogicValid;
            foreach (LogicEntity output in logicEntity.Outputs)
            {
                if (newOutput && !output.UseSeparateOutputLogic)
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
