using TMPro;
using UnityEngine;

namespace Codebase.Gameplay.Sorting
{
    public class SortableDebug : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _positionInfo;
        [SerializeField] private TextMeshPro _orderInfo;

        public void UpdateDebugView(float position, int order)
        {
            _positionInfo.text = "pos: " + position;
            _orderInfo.text = "ord: " + order;
        }
    }
}