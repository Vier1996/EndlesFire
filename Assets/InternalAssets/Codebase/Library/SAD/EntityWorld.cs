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
        
        /*public List<Entity> GetEntitiesByDistance(Vector3 point, float distance) =>
            m_AliveEntities
                .Where(en => Vector3.Distance(point, en.transform.position) <= distance)
                .ToList();
        
        public T GetClosestEntity<T>(Vector3 point, float distance) where T : class
        {
            for (int i = 0; i < m_AliveEntities.Count; i++)
            {
                if (m_AliveEntities[i] is not T)
                    continue;
            }
            return m_AliveEntities
                .Where(en => (en is T) && Vector3.Distance(point, en.transform.position) <= distance)
                .OrderBy(en => Vector3.Distance(point, en.transform.position))
                .FirstOrDefault() as T;
        }

        public T GetClosestEntity<T>(Vector3 point, double distance) where T : class =>
            m_AliveEntities
                .Where(en => (en is T) && Vector3.Distance(point, en.transform.position) <= distance)
                .OrderBy(en => Vector3.Distance(point, en.transform.position))
                .FirstOrDefault() as T;
        
        public List<Entity> FilterByDistance(List<Entity> inputEntities, Vector3 point, float distance) =>
            inputEntities
                .Where(en => Vector3.Distance(point, en.transform.position) <= distance)
                .ToList();
        
        public List<Entity> GetEntitiesByEntityType<T>() =>
            m_AliveEntities
                .Where(en => en is T)
                .ToList();
        
        public List<Entity> GetEntitiesByEntityComponent<T>() =>
            m_AliveEntities
                .Where(en => en is T)
                .ToList();

        public List<Entity> GetAllEntities() => m_AliveEntities;*/
    }
}
