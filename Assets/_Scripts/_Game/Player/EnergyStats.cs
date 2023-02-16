using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Scripts._Game.Player{
    
    public class EnergyStats
    {
        private float _energyPoints;
        private float _maxEnergyPoints;

        public float EnergyPoints { get => _energyPoints; }

        public EnergyStats(float energyPoints, float maxEnergyPoints)
        {
            _energyPoints = energyPoints;
            _maxEnergyPoints = maxEnergyPoints;
        }

        public float AddEnergyPoints(float amount)
        {
            float clampedAmount = Mathf.Min(amount, _maxEnergyPoints - _energyPoints);

            _energyPoints += clampedAmount;

            return _energyPoints;
        }

        public float RemoveEnergyPoints(float amount, bool react = false)
        {
            float clampedAmount = Mathf.Min(amount, _energyPoints);

            _energyPoints -= clampedAmount;

            return _energyPoints;
        }
    }
    
}
