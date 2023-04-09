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
        private Dictionary<string, SpawnPoint> _spawnPointsDict;
        private Dictionary<string, Waypoints> _waypointsDict;
        #endregion

        protected override void Awake()
        {
            base.Awake();

            _spawnPointsDict = new Dictionary<string, SpawnPoint>();
            _waypointsDict = new Dictionary<string, Waypoints>();
        }

        #region Spawnpoints
        public void RegisterRuntimeSpawnPoint(SpawnPoint spawnPoint)
        {
            if (!_spawnPointsDict.ContainsKey(spawnPoint.RuntimeID.Id) || _spawnPointsDict[spawnPoint.RuntimeID.Id] == null)
            {
                Log("Registering spawnpoint: " + spawnPoint.RuntimeID.Id + ".");
                _spawnPointsDict.Add(spawnPoint.RuntimeID.Id, spawnPoint);
            }
        }

        public void UnregisterRuntimeSpawnPoint(SpawnPoint spawnPoint)
        {
            if (_spawnPointsDict.ContainsKey(spawnPoint.RuntimeID.Id))
            {
                Log("Unregistering spawnpoint: " + spawnPoint.RuntimeID.Id + ".");
                _spawnPointsDict.Remove(spawnPoint.RuntimeID.Id);
            }
        }

        public SpawnPoint GetRuntimeSpawnPoint(string id)
        {
            SpawnPoint spawnPoint = null;
            _spawnPointsDict.TryGetValue(id, out spawnPoint);
            return spawnPoint;
        }
        #endregion

        #region Waypoints
        public void RegisterRuntimeWaypoint(Waypoints waypoint)
        {
            if (!_waypointsDict.ContainsKey(waypoint.RuntimeID.Id) || _waypointsDict[waypoint.RuntimeID.Id] == null)
            {
                Log("Unregistering waypoint: " + waypoint.RuntimeID.Id + ".");
                _waypointsDict.Add(waypoint.RuntimeID.Id, waypoint);
            }
        }

        public void UnregisterRuntimeWaypoint(Waypoints waypoint)
        {
            if (_waypointsDict.ContainsKey(waypoint.RuntimeID.Id))
            {
                Log("Unregistering waypoint: " + waypoint.RuntimeID.Id + ".");
                _waypointsDict.Remove(waypoint.RuntimeID.Id);
            }
        }

        public Waypoints GetRuntimeWaypoints(string id)
        {
            Waypoints wayPoint = null;
            _waypointsDict.TryGetValue(id, out wayPoint);
            return wayPoint;
        }
        #endregion

        #region Debug
        private void Log(string log)
        {
            Debug.Log("RuntimeIdManager: " + log);
        }

        private void LogWarning(string log)
        {
            Debug.LogWarning("RuntimeIdManager: " + log);
        }
        #endregion
    }
}
