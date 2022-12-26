﻿using Client.Painting;
using Core.UserInput;
using UnityEngine;

namespace Client.Cursors
{
    public class JoystickInputHandler : ICursorInputHandler
    {
        private readonly Painter painter;
        private readonly Joystick joystick;
        private readonly Rigidbody2D cursorRb;
        private readonly JoystickInputConfig joystickInputConfig;

        public JoystickInputHandler(Painter painter, Joystick joystick, Rigidbody2D cursorRb, JoystickInputConfig joystickInputConfig)
        {
            this.painter = painter;
            this.joystick = joystick;
            this.cursorRb = cursorRb;
            this.joystickInputConfig = joystickInputConfig;
        }

        public void HandlePointerDown(Vector2 inputPos)
        {
            joystick.OnPointerDown(inputPos);
        }

        public void HandlePointerMove(Vector2 inputPos)
        {
            joystick.OnPointerMove(inputPos);
            var axis = joystick.Value;
            cursorRb.MovePosition(cursorRb.transform.position + (Vector3)axis * Time.deltaTime * joystickInputConfig.Speed);
            
            painter.Paint(cursorRb.transform.position);
        }

        public void HandlePointerUp(Vector2 inputPos)
        {
            joystick.OnPointerUp(inputPos);
        }
    }
}