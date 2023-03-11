using _Scripts._Game.AI;
using _Scripts._Game.General.Identification;
using _Scripts._Game.General.Spawning.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.General.Managers{
    // this clasas is to keep track of all the ids in each scene and update them and their resective references during runtime
    public class RuntimeIDManager : Singleton<RuntimeIDManager>
    {
        #region General
        private Dictionary<string, SpawnPoint> _spawnPointsDict = new Dictionary<string, SpawnPoint>();
        private Dictionary<string, Waypoints> _waypointsDict = new Dictionary<string, Waypoints>();
        #endregion
    }

}

public interface IRuntimeId
{
    public RuntimeID RuntimeID { get; }
}
