using UnityEngine;

namespace Codebase.Library.Areas
{
    public class Area : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private Color _areaColor;
#endif
        [SerializeField] private Vector3 _areaSize;

        private Transform _selfTransform;

        private void Awake() => _selfTransform = transform;

        public Vector3 GetRandomPosition()
        {
            Vector3 halfSize = new Vector3(_areaSize.x / 2, _areaSize.y / 2, _areaSize.z / 2);
            Vector3 localRandomPoint = new Vector3(
                UnityEngine.Random.Range(-halfSize.x, halfSize.x), 
                UnityEngine.Random.Range(-halfSize.y, halfSize.y),
                UnityEngine.Random.Range(-halfSize.z, halfSize.z));
            return _selfTransform.TransformPoint(localRandomPoint);
        }
        
        public bool InBounds(Vector3 position)
        {
            Vector3 halfSize = new Vector3(_areaSize.x / 2, _areaSize.y / 2, _areaSize.z / 2);

            bool inBoundsX = position.x >= -halfSize.x && position.x <= halfSize.x;
            bool inBoundsZ = position.z >= -halfSize.z && position.z <= halfSize.z;

            return inBoundsX && inBoundsZ;
        }
        
#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            if(Application.isPlaying)
                Gizmos.color = new Color(_areaColor.r, _areaColor.g, _areaColor.b, 0.05f);
            else
                Gizmos.color = _areaColor;

            Matrix4x4 defaultGizmosMatrix = Gizmos.matrix;
            Gizmos.matrix = transform.localToWorldMatrix;
            Gizmos.DrawCube(Vector3.zero, _areaSize);
            Gizmos.matrix = defaultGizmosMatrix;
        }
#endif
    }
}