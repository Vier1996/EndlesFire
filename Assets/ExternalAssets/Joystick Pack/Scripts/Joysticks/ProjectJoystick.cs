using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Plugins.Joystick_Pack.Scripts.Joysticks
{
    public class ProjectJoystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
    {
        public static float SHorizontal { get; private set; }
        public static float SVertical { get; private set; }
        public static Vector2 SDirection => new Vector2(SHorizontal, SVertical);
       
        public static PointerEventData LastPoint { get; private set; }

        public  static Vector2 JoystickCenter { get; private set; }
        
            public float Horizontal
        {
            get { return (snapX) ? SnapFloat(input.x, AxisOptions.Horizontal) : input.x; }
        }

        public float Vertical
        {
            get { return (snapY) ? SnapFloat(input.y, AxisOptions.Vertical) : input.y; }
        }

        public Vector2 Direction
        {
            get { return new Vector2(Horizontal, Vertical); }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                if (value == false)
                    OnPointerUp(new PointerEventData(EventSystem.current));
                _isActive = value;
            }
        }

        public float HandleRange
        {
            get { return handleRange; }
            set { handleRange = Mathf.Abs(value); }
        }

        public float DeadZone
        {
            get { return deadZone; }
            set { deadZone = Mathf.Abs(value); }
        }

        public void RefreshValues()
        {
            SHorizontal = 0f;
            SVertical = 0f;
        }

        public AxisOptions AxisOptions
        {
            get { return AxisOptions; }
            set { axisOptions = value; }
        }

        public bool SnapX
        {
            get { return snapX; }
            set { snapX = value; }
        }

        public bool SnapY
        {
            get { return snapY; }
            set { snapY = value; }
        }

        [SerializeField] private float handleRange = 1;
        [SerializeField] private float deadZone = 0;
        [SerializeField] private AxisOptions axisOptions = AxisOptions.Both;
        [SerializeField] private bool snapX = false;
        [SerializeField] private bool snapY = false;

        [HideInInspector] public int onUp;
        [HideInInspector] public int onDown;

        [SerializeField] protected RectTransform background = null;
        [SerializeField] private RectTransform handle = null;
        [SerializeField] private RectTransform _outlineTransform = null;
        [SerializeField] private RectTransform _highlightParentTransform = null;
        
        [SerializeField] private CanvasGroup _outlineGroup;
        [SerializeField] private CanvasGroup _highlightGroup;
        [SerializeField] private CanvasGroup joystickGroup = null;
        
        [SerializeField] private bool _tutorialBehaviour = true;
        [ShowIf("@_tutorialBehaviour == true"), SerializeField] private RectTransform _tutorialParent = null;

        private RectTransform baseRect = null;
        private Canvas canvas;
        private Camera cam;

        private Vector2 input = Vector2.zero;
        private bool _isActive = true;

        public void SetTutorialBehavior() => _tutorialBehaviour = true;
        
        public void SetDefaultBehavior()
        {
            _tutorialBehaviour = false;
        }

        protected virtual void Start()
        {
            HandleRange = handleRange;
            DeadZone = deadZone;
            baseRect = GetComponent<RectTransform>();
            canvas = GetComponentInParent<Canvas>();

            Vector2 center = new Vector2(0.5f, 0.5f);
            background.pivot = center;
            handle.anchorMin = center;
            handle.anchorMax = center;
            handle.pivot = center;
            handle.anchoredPosition = Vector2.zero;

            OnPointerDown(new PointerEventData(EventSystem.current));
            OnPointerUp(new PointerEventData(EventSystem.current));
            
            if(!_tutorialBehaviour)
                background.gameObject.SetActive(false);
            else
            {
                Vector3 parentPosition = _tutorialParent.localPosition;
                background.localPosition = new Vector3(parentPosition.x, parentPosition.y + 35, parentPosition.z);
                _tutorialParent.SetParent(background.transform);
            }
            
            _outlineGroup.alpha = 0;
            _highlightGroup.alpha = 0;
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            if (IsActive == false) return;

            if (!_tutorialBehaviour)
            {
                background.gameObject.SetActive(true);
            }

            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            
            if (joystickGroup != null)
                joystickGroup.alpha = 1f;

            OnDrag(eventData);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (IsActive == false) return;
            cam = null;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                cam = canvas.worldCamera;

            Vector2 position = RectTransformUtility.WorldToScreenPoint(cam, background.position);
            Vector2 radius = background.sizeDelta / 2;
            input = (eventData.position - position) / (radius * canvas.scaleFactor);

            LastPoint = eventData;
            JoystickCenter = position;
            FormatInput();
            HandleInput(input.magnitude, input.normalized, radius, cam);
            
            handle.anchoredPosition = input * radius * handleRange;
            
            SetStaticInput();
            RotateExtras();
        }

        private void RotateExtras()
        {
            float angle = Mathf.Atan2(input.normalized.y, input.normalized.x) * Mathf.Rad2Deg;
            
            if (angle < 0) angle += 360;
            angle -= 90;
            
            _outlineTransform.rotation = Quaternion.Euler(0, 0, angle);
            _highlightParentTransform.rotation = Quaternion.Euler(0, 0, angle);
        }

        protected virtual void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
        {
            if (magnitude > deadZone)
            {
                if (magnitude > 1)
                    input = normalised;
            }
            else
                input = Vector2.zero;
            
            _outlineGroup.alpha = magnitude;
            _highlightGroup.alpha = magnitude;
        }

        private void FormatInput()
        {
            if (axisOptions == AxisOptions.Horizontal)
                input = new Vector2(input.x, 0f);
            else if (axisOptions == AxisOptions.Vertical)
                input = new Vector2(0f, input.y);
        }

        private float SnapFloat(float value, AxisOptions snapAxis)
        {
            if (value == 0)
                return value;

            if (axisOptions == AxisOptions.Both)
            {
                float angle = Vector2.Angle(input, Vector2.up);
                if (snapAxis == AxisOptions.Horizontal)
                {
                    if (angle < 22.5f || angle > 157.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }
                else if (snapAxis == AxisOptions.Vertical)
                {
                    if (angle > 67.5f && angle < 112.5f)
                        return 0;
                    else
                        return (value > 0) ? 1 : -1;
                }

                return value;
            }
            else
            {
                if (value > 0)
                    return 1;
                if (value < 0)
                    return -1;
            }

            return 0;
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (IsActive == false) return;

            if(!_tutorialBehaviour)
                background.gameObject.SetActive(false);

            if (joystickGroup != null)
                joystickGroup.alpha = 1f;

            input = Vector2.zero;
            handle.anchoredPosition = Vector2.zero;
            SetStaticInput();
        }

        private void SetStaticInput()
        {
            SHorizontal = Horizontal;
            SVertical = Vertical;
        }

        protected Vector2 ScreenPointToAnchoredPosition(Vector2 screenPosition)
        {
            Vector2 localPoint = Vector2.zero;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(baseRect, screenPosition, cam, out localPoint))
            {
                Vector2 pivotOffset = baseRect.pivot * baseRect.sizeDelta;
                return localPoint - (background.anchorMax * baseRect.sizeDelta) + pivotOffset;
            }

            return Vector2.zero;
        }
    }

    public enum AxisOptions
    {
        Both,
        Horizontal,
        Vertical
    }
}
