using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class TargetManager : Singleton<TargetManager> 
    {
        private ITarget[] _targets;

        private void Start()
        {
            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();
            _targets = new ITarget[targets.Count()];

            for (int i = 0; i < targets.Count(); ++i)
            {
                _targets[i] = targets.ElementAt(i);
            }
        }
    }
    
    public interface ITarget
    {
        public Transform GetTargetTransform();
    }
}
