using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Codebase.Library.Resizers
{
    public class CinemachineCameraResizer : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _virtualCamera;
        [SerializeField] private bool _useCustomOrtoSize;
        
        [ShowIf(nameof(_useCustomOrtoSize))]
        [SerializeField] private float _customOrtoSize;
        
        private float _baseWidth = 7;

        private void Awake() => SetupCamera();

        private void SetupCamera()
        {
            if (_useCustomOrtoSize)
            {
                _virtualCamera.m_Lens.OrthographicSize = _customOrtoSize;
                return;
            }
            
            if ((float) Screen.height / Screen.width > 1.4f)
            {
                var ortoSize = _baseWidth * 0.5f / Screen.width * Screen.height;
                
                if(ortoSize < 6) 
                    ortoSize = _baseWidth;
                
                _virtualCamera.m_Lens.OrthographicSize = ortoSize;
            }
        }
    }
}