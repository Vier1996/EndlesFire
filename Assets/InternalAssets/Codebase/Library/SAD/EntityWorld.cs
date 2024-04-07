using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<Entity> GetEntitiesByDistance(Vector3 point, float distance) =>
            m_AliveEntities
                .Where(en => Vector3.Distance(point, en.transform.position) <= distance)
                .ToList();
        
        public T GetClosestEntity<T>(Vector3 point, float distance) where T : class =>
            m_AliveEntities
                .Where(en => (en is T) && Vector3.Distance(point, en.transform.position) <= distance)
                .OrderBy(en => Vector3.Distance(point, en.transform.position))
                .FirstOrDefault() as T;
        
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

        public List<Entity> GetAllEntities() => m_AliveEntities;
    }
}
