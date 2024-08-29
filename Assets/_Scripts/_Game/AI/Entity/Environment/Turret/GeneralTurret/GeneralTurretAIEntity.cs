using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Environment.Turret.GeneralTurret{
    
    public class GeneralTurretAIEntity : TurretBaseAIEntity
    {
       public GeneralTurretAIEntity() 
       {
            _entity = General.EEntity.GeneralTurret;
       }
    }
    
}
