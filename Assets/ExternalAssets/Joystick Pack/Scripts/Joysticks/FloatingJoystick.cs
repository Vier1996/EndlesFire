using System.Collections;
using System.Collections.Generic;
using Plugins.Joystick_Pack.Scripts.Base;
using UnityEngine;
using UnityEngine.EventSystems;

public class FloatingJoystick : Joystick
{
    
    protected override void Start()
    {
        base.Start();
        background.gameObject.SetActive(false);
    }

    public void AdaptiveToTouch()
    {
        background.gameObject.SetActive(true);
        OnPointerDown(new PointerEventData(EventSystem.current));
        OnDrag(new PointerEventData(EventSystem.current));
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        background.anchoredPosition = ScreenPointToAnchoredPosition(eventData.position);
        background.gameObject.SetActive(true);
        base.OnPointerDown(eventData);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        background.gameObject.SetActive(false);
        base.OnPointerUp(eventData);
    }
}