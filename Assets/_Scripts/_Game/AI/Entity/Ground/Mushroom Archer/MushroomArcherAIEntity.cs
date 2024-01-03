using _Scripts._Game.General;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.AI.Entity.Ground.MushroomArcher{
    
    public class MushroomArcherAIEntity : AIEntity
    {
       public MushroomArcherAIEntity()
       {
            _entity = EEntity.MushroomArcher;
       }
    
       protected override void Awake()
       {
            base.Awake();
       }
    
       public override void Tick()
       {
            base.Tick();
       }
    }
    
    
    
}
