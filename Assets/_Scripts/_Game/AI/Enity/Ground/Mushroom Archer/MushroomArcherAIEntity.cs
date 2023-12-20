using System.Collections;
using System.Collections.Generic;
using _Scripts._Game.General;
using UnityEngine;

namespace _Scripts._Game.AI.Enity.Ground.MushroomArcher{
    
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
    
       protected override void FixedUpdate()
       {
            base.FixedUpdate();
       }
    }
    
    
    
}
