using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General.SaveLoad;
using UnityEngine;
using UnityEngine.UI;
using  _Scripts._Game.General.Managers;

namespace _Scripts._Game.UI.MainMenu{
    
    public class SaveSlotsMenu : MonoBehaviour
    {
        [SerializeField]
        private Button[] _saveSlotButtons = new Button[3];

        void OnEnable()
        {
            RefreshSaveSlotsView();
        }

        private void RefreshSaveSlotsView()
        {
            for (int i = 0; i < _saveSlotButtons.Length; i++)
            {
                bool enable = false;
                // see if save files exist and have any play time to enable/disable button
                object saveObj = SaveLoadSystem.Instance.RetrieveLoadObject(General.ELoadSpecifier.PlayTime, i);
                if (saveObj != null)
                {
                    StatsManager.SaveData statsData = (StatsManager.SaveData)saveObj;
                    enable = (statsData._completionStats[(int)EStatsType.TimePlayed] > 0);

                    _saveSlotButtons[i].interactable = enable;
                }
                else
                {
                    Debug.Log("No saveObj retrieved");
                    _saveSlotButtons[i].interactable = false;
                }
            }
        }
    }
    
}
