using System;
using Plugins.Joystick_Pack.Scripts.Joysticks;
using UnityEngine;

namespace InternalAssets.Codebase.Services.Input
{
    public class MobilePlayerInputService : IInputService
    {
        public MobilePlayerInputService()
        {
            
        }

        public Vector2 Axis
        {
            get
            {
                return ProjectJoystick.SDirection;
            }
        }

        public float Vertical
        {
            get
            {
                return ProjectJoystick.SVertical;
            }
        }

        public float Horizontal
        {
            get
            {
                return ProjectJoystick.SHorizontal;
            }
        }

        public float DragDistance
        {
            get
            {
                if (ProjectJoystick.LastPoint == null || ProjectJoystick.JoystickCenter == null) return 0f;
                
                return (float)Math.Round(
                    Vector2.Distance(ProjectJoystick.LastPoint.position,ProjectJoystick.JoystickCenter)
                    /100,1);
            }
        }
    }
}