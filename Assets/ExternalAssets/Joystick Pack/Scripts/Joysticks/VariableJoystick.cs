using Plugins.Joystick_Pack.Scripts.Base;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VariableJoystick : Joystick
{
    private const string UILayerName = "WorldUI";

    public float MoveThreshold { get { return moveThreshold; } set { moveThreshold = Mathf.Abs(value); } }

    [SerializeField] private float moveThreshold = 1;
    [SerializeField] private JoystickType joystickType = JoystickType.Fixed;
    [SerializeField] private Image image;
    [SerializeField] [Range(0f, 1f)] private float _idleStateTransparency;
    private CanvasGroup _backgroundCanvasGroup;
    
    private RaycastHit hit;
    private Ray ray;

    private Vector2 fixedPosition = Vector2.zero;

    public void SetMode(JoystickType joystickType)
    {
        this.joystickType = joystickType;
        if(joystickType == JoystickType.Fixed)
        {
            background.anchoredPosition = fixedPosition;
            _backgroundCanvasGroup.alpha = 1f;

        }
        else
        {
            background.anchoredPosition = fixedPosition;
            _backgroundCanvasGroup.alpha = _idleStateTransparency;
        }

    }

    private void Awake()
    {
        image = GetComponent<Image>();
        _backgroundCanvasGroup = background.GetComponent<CanvasGroup>();
    }

    protected override void Start()
    {
        base.Start();
        fixedPosition = background.anchoredPosition;
        SetMode(joystickType);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        ray =  Camera.main!.ScreenPointToRay(eventData.position);
        if ( Physics.Raycast(ray, out hit, Mathf.Infinity,LayerMask.GetMask(UILayerName)))
        {
            eventData.selectedObject = null;
            image.enabled = false;
            hit.collider.GetComponent<Button>().onClick.Invoke();
            image.enabled = true;
            return;
        }
        
        if(joystickType != JoystickType.Fixed)
        {
            background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
            _backgroundCanvasGroup.alpha = 1f;
        }
        
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (joystickType != JoystickType.Fixed)
        {
            // background.gameObject.SetActive(false);
            _backgroundCanvasGroup.alpha = _idleStateTransparency;
            background.anchoredPosition = fixedPosition;
        }

        image.enabled = true;

        base.OnPointerUp(eventData);
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (hit.collider == null)
        {
            base.OnDrag(eventData);
        }
    }

    private void AdsHelperOnInterstitialStarts() => OnPointerUp(null);

    protected override void HandleInput(float magnitude, Vector2 normalised, Vector2 radius, Camera cam)
    {
        if (hit.collider == null)
        {
            if (joystickType == JoystickType.Dynamic && magnitude > moveThreshold)
            {
                Vector2 difference = normalised * (magnitude - moveThreshold) * radius;
                background.anchoredPosition += difference;
            }
            base.HandleInput(magnitude, normalised, radius, cam);
        }
    }
}

public enum JoystickType { Fixed, Floating, Dynamic }