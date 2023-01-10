using _Scripts._Game.AI.MovementStateMachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public enum ETargetType
    {
        AbovePlayer,
    }

    public class TargetManager : Singleton<TargetManager> 
    {
        private Dictionary<ETargetType, ITarget> _targetsDict = new Dictionary<ETargetType, ITarget>();

        private void Start()
        {
            var targets = FindObjectsOfType<MonoBehaviour>().OfType<ITarget>();

            for (int i = 0; i < targets.Count(); ++i)
            {
                _targetsDict.Add(targets.ElementAt(i).TargetType, targets.ElementAt(i));
            }
        }
    }
    
    public interface ITarget
    {
        public ETargetType TargetType { get; }
        public Transform GetTargetTransform();
    }
}
