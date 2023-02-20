using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using _Scripts._Game.AI;

namespace _Scripts._Game.General.Managers{
    
    public class CorpsePool : PoolComponentManager<Corpse>
    {
        protected override bool IsActive(Corpse component)
        {
            return false;
        }
    }
    
}
