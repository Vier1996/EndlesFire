using UnityEngine;

namespace Codebase.Library.Resizers
{
    public class CameraResizer : MonoBehaviour
    {
        [SerializeField] private float _baseWidth = 7;

        private void Awake() => SetupCamera();

        private void SetupCamera()
        {
            if ((float) Screen.height / Screen.width > 1.4f)
            {
                var ortoSize = _baseWidth * 0.5f / Screen.width * Screen.height;
                if(ortoSize < 6) ortoSize = _baseWidth;
                GetComponent<UnityEngine.Camera>().orthographicSize = ortoSize;
            }
        }
    }
}
