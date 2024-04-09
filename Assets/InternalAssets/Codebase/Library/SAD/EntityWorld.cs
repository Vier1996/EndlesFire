using System;
using System.Collections.Generic;
using System.Linq;
using InternalAssets.Codebase.Library.Vectors;
using JetBrains.Annotations;
using UnityEngine;

namespace Codebase.Library.SAD
{
    public class EntityWorld
    {
        private readonly List<Entity> m_AliveEntities = new List<Entity>();
        
        public void AddEntity(Entity entity)
        {
            if(m_AliveEntities.Contains(entity) == false)
                m_AliveEntities.Add(entity);
        }
        
        public void RemoveEntity(Entity entity) 
        {
            if(m_AliveEntities.Contains(entity))
                m_AliveEntities.Remove(entity);
        }

        [CanBeNull]
        public T GetClosestEntity<T>(Vector3 point, float distance) where T : Entity
        {
            T outputEntity = null;
            float minimalLength = 100f;
            
            for (int i = 0; i < m_AliveEntities.Count; i++)
            {
                T entity = m_AliveEntities[i] as T;
                
                if (entity == null)
                    continue;

                float distanceBetweenEntities = point.DistanceXY(entity.Transform.position);

                if (distanceBetweenEntities <= distance && distanceBetweenEntities < minimalLength)
                {
                    outputEntity = entity;
                    minimalLength = distanceBetweenEntities;
                }
            }

            return outputEntity;
        }
        
        [CanBeNull]
        public Entity GetClosestEntity(List<Type> entityTypes, Vector3 point, float distance)
        {
            Entity outputEntity = null;
            float minimalLength = 100f;
            
            for (int i = 0; i < m_AliveEntities.Count; i++)
            {
                Entity entity = m_AliveEntities[i];
                
                if(entityTypes.Contains(entity.GetType()) == false)
                    continue;
                
                if (entity == null)
                    continue;

                float distanceBetweenEntities = point.DistanceXY(entity.Transform.position);

                if (distanceBetweenEntities <= distance && distanceBetweenEntities < minimalLength)
                {
                    outputEntity = entity;
                    minimalLength = distanceBetweenEntities;
                }
            }

            return outputEntity;
        }
    }
}
