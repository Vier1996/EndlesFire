using UnityEngine;

namespace Codebase.Library.Collider
{
    public class CollidersGizmosDrawer : MonoBehaviour
    {
        private Color _color;
        
        public void SetColor(Color color) => _color = color;

#if UNITY_EDITOR
        public void OnDrawGizmos()
        {
            var colliders = GetComponentsInChildren<BoxCollider>();
            foreach (BoxCollider boxCollider in colliders)
            {
                var defaultGizmosMatrix = Gizmos.matrix;
                var colliderSize = boxCollider.size;
                Gizmos.color = _color;
                Gizmos.matrix = boxCollider.transform.localToWorldMatrix;
                Gizmos.DrawCube(boxCollider.center, new Vector3(colliderSize.x, colliderSize.y, colliderSize.z));
                Gizmos.matrix = defaultGizmosMatrix;
            }
        }
#endif
    }
}
