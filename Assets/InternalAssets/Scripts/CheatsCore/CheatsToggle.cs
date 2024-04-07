using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace CheatsCore
{
    internal class CheatsToggle : MonoBehaviour, ICheatsElement
    {
        public Toggle Toggle;

        [SerializeField] private TextMeshProUGUI captionLabel;

        [SerializeField] private int MinPreferredWidth = 250;

        public Func<bool> IsEnabledCallback { get; set; }
        
        public RectTransform RectTransform { get; private set; }

        public void SetCaption(string caption)
        {
            captionLabel.text = caption;

            SetWidth(GetWidth());
        }

        public void SetWidth(float width)
        {
            RectTransform.SetWidth(width);
        }

        public float GetWidth() => Mathf.Max(Cheats.CalcTextWidth(captionLabel.text), MinPreferredWidth);
        
        private void Awake()
        {
            RectTransform = GetComponent<RectTransform>();
        }
    }
}