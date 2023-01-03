using Ji2Core.Core;
using Ji2Core.Core.UserInput;

namespace Client.Cursors
{
    public class CursorInputHandlerFactory
    {
        private readonly Context context;
        private readonly JoystickInputConfig joystickInputConfig;
        private readonly DirectCursorInputHandlerConfig directCursorInputHandlerConfig;

        public CursorInputHandlerFactory(Context context, JoystickInputConfig joystickInputConfig, DirectCursorInputHandlerConfig directCursorInputHandlerConfig)
        {
            this.context = context;
            this.joystickInputConfig = joystickInputConfig;
            this.directCursorInputHandlerConfig = directCursorInputHandlerConfig;
        }

        public ICursorInputHandler Create<TInputHandler>() where TInputHandler : class, ICursorInputHandler
        {
            if (typeof(TInputHandler) == typeof(DirectCursorInputHandler))
            {
                return new DirectCursorInputHandler(context.GetService<CameraProvider>(),
                    context.GetService<CursorService>().cursorRigidbody, directCursorInputHandlerConfig);
            }
            else
            {
                return new JoystickInputHandler(context.GetService<Joystick>(), 
                    context.GetService<CursorService>().cursorRigidbody, joystickInputConfig);
            }
        }
    }
}