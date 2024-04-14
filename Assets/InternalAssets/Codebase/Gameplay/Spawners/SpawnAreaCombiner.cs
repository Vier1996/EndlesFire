using System.Collections.Generic;
using Codebase.Library.Random;
using UnityEngine;

namespace InternalAssets.Codebase.Gameplay.Spawners
{
    public class SpawnAreaCombiner : MonoBehaviour
    {
        [SerializeField] private List<SpawnerArea> _areas = new();
        
        public bool TryGetAvailablePoint(Vector3 boxSize, bool checkVisible, out Vector3 position)
        {
            int maxIterationCount = 15;
            position = Vector3.zero;
            
            while (maxIterationCount >= 0)
            {
                SpawnerArea area = _areas.Random();
                
                if(area == null)
                    continue;
                
                Vector3 possiblePoint = area.GetAvailablePoint(boxSize, 0.5f, checkVisible);

                if (possiblePoint != Vector3.zero)
                {
                    position = possiblePoint;
                    return true;
                }

                maxIterationCount--;
            }
            
            return false;
        }
    }
}