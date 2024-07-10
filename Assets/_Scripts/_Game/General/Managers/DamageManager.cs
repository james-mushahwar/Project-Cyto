using _Scripts._Game.AI;
using _Scripts._Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    
    public class DamageManager : Singleton<AssetManager>, IManager
    {
        //statics
        public static float GetDamageFromTypeToEntity(EDamageType damageType, IDamageable damageable)
        {
            float damage = 0.0f;

            PlayerEntity playerEntity = damageable as PlayerEntity;
            AIEntity aiEntity = damageable as AIEntity;

            if (playerEntity != null)
            {
                damage = 1.0f;

                switch (damageType)
                {
                    case EDamageType.Laser_Instakill:
                        damage = playerEntity.PlayerHealthStats.HitPoints;
                        break;
                    default: 
                        break;
                }
            }
            else if (aiEntity != null)
            {
                damage = 1.0f;

                switch (damageType)
                {
                    case EDamageType.Laser_Instakill:
                        damage = aiEntity.EnemyHealthStats.HitPoints;
                        break;
                    default:
                        break;
                }
            }


            return damage;
        }

        public static bool CanBeDamaged(EDamageType damageType, IDamageable damageable)
        {
            if (damageable.DamageTypesToAccept.Count > 0)
            {
                if (damageable.DamageTypesToAccept.Contains(damageType) == false)
                {
                    return false;
                }
            }

            if (damageable.DamageTypesToIgnore.Contains(damageType))
            {
                return false;
            }

            return true;
        }
    }
    
}
