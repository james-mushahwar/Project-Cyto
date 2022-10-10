using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.SaveLoad{

    public interface ISaveable
    {
        object SaveState();

        void LoadState(object state);

    }
}
