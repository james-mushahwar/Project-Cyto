using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Projectile.Environment{
    
    public interface GroupSelector
    {
        public void SetIndex(int index) { }
        public void OnIndexChanged() { }
    }
    
}
