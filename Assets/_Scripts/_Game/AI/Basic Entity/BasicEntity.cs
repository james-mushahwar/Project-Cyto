using _Scripts._Game.General;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace _Scripts._Game.AI.BasicEntity{

    public abstract class BasicEntity : MonoBehaviour, IDamageable
    {
        public virtual IExposable Exposable  { get => throw new System.NotImplementedException(); }

        public virtual Vector2 DamageDirection { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public virtual List<EDamageType> DamageTypesToIgnore => throw new System.NotImplementedException();

        public virtual List<EDamageType> DamageTypesToAccept => throw new System.NotImplementedException();

        public virtual Transform Transform => throw new System.NotImplementedException();

        public virtual EEntityType EntityType => throw new System.NotImplementedException();

        public virtual bool IsAlive()
        {
            throw new System.NotImplementedException();
        }

        public virtual bool TakeDamage(EDamageType damageType, EEntityType causer, Vector3 damagePosition)
        {
            throw new System.NotImplementedException();
        }
    }

}
