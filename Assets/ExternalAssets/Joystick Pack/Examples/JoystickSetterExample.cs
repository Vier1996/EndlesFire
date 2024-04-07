using System.Collections;
using System.Collections.Generic;
using Plugins.Joystick_Pack.Scripts.Base;
using UnityEngine;
using UnityEngine.UI;

public class JoystickSetterExample : MonoBehaviour
{
    public VariableJoystick variableMoveController;
    public Text valueText;
    public Image background;
    public Sprite[] axisSprites;

    public void ModeChanged(int index)
    {
        switch(index)
        {
            case 0:
                variableMoveController.SetMode(JoystickType.Fixed);
                break;
            case 1:
                variableMoveController.SetMode(JoystickType.Floating);
                break;
            case 2:
                variableMoveController.SetMode(JoystickType.Dynamic);
                break;
            default:
                break;
        }     
    }

    public void AxisChanged(int index)
    {
        switch (index)
        {
            case 0:
                variableMoveController.AxisOptions = AxisOptions.Both;
                background.sprite = axisSprites[index];
                break;
            case 1:
                variableMoveController.AxisOptions = AxisOptions.Horizontal;
                background.sprite = axisSprites[index];
                break;
            case 2:
                variableMoveController.AxisOptions = AxisOptions.Vertical;
                background.sprite = axisSprites[index];
                break;
            default:
                break;
        }
    }

    public void SnapX(bool value)
    {
        variableMoveController.SnapX = value;
    }

    public void SnapY(bool value)
    {
        variableMoveController.SnapY = value;
    }

    private void Update()
    {
        valueText.text = "Current Value: " + variableMoveController.Direction;
    }
}