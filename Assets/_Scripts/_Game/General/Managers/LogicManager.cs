using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.LogicController;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class LogicManager : Singleton<LogicManager>, IManager
    {
        //private List<bool> validInputs = new List<bool>();
        //private List<bool>

        public bool AreAllInputsValid(ILogicEntity logicEntity)
        {
            bool inputIsValid = true;
            bool entityLogicIsValid = logicEntity.IsEntityLogicValid != null ? logicEntity.IsEntityLogicValid.Invoke() : true;

            if (logicEntity.UseAdvancedInputs)
            {
                List<bool> validInputs = new List<bool>();
                foreach(FLogicSignal advancedInput in logicEntity.AdvancedInputs)
                {
                    bool validLogicSignal = false;
                    List<bool> validSignals = new List<bool>();

                    foreach (LogicEntity input in advancedInput.LogicEntities)
                    {
                        validSignals.Add(input.IsOutputLogicValid);
                    }

                    if (advancedInput.ConditionType == ELogicSignalConditionType.OR)
                    {
                        validLogicSignal = validSignals.Contains(true);
                    }
                    else if (advancedInput.ConditionType == ELogicSignalConditionType.AND)
                    {
                        validLogicSignal = !validSignals.Contains(false);
                    }

                    validInputs.Add(validLogicSignal);
                }

                if (logicEntity.AdvancedConditionType == ELogicSignalConditionType.OR)
                {
                    inputIsValid = validInputs.Contains(true);
                }
                else if (logicEntity.AdvancedConditionType == ELogicSignalConditionType.AND)
                {
                    inputIsValid = !validInputs.Contains(false);
                }

                return inputIsValid;
            }
            else
            {
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
            }

            return inputIsValid && entityLogicIsValid;
        }

        public void OnOutputChanged(ILogicEntity logicEntity)
        {
            bool newOutput = logicEntity.IsOutputLogicValid;
            foreach (LogicEntity output in logicEntity.Outputs)
            {
                if (!output.UseSeparateOutputLogic)
                {
                    //if (AreAllInputsValid(output) != output.IsInputLogicValid)
                    {
                        output.IsInputLogicValid = AreAllInputsValid(output);
                        //output.OnInputChanged.Invoke();
                        //OnOutputChanged(output);
                    }
                }
            }
        }

        public void ManagedPreInGameLoad()
        {
             
        }

        public void ManagedPostInGameLoad()
        {
             
        }

        public void ManagedPreMainMenuLoad()
        {
             
        }

        public void ManagedPostMainMenuLoad()
        {
             
        }
    }
    
}
