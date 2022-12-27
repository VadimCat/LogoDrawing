using Client.Painting;
using Core;
using Core.Camera;
using Core.UserInput;
using Cysharp.Threading.Tasks;

namespace Client.Cursors
{
    public class CursorInputHandlerFactory
    {
        private readonly Context context;
        private readonly JoystickInputConfig joystickInputConfig;

        public CursorInputHandlerFactory(Context context, JoystickInputConfig joystickInputConfig)
        {
            this.context = context;
            this.joystickInputConfig = joystickInputConfig;
        }

        public ICursorInputHandler Create<TInputHandler>() where TInputHandler : class, ICursorInputHandler
        {
            if (typeof(TInputHandler) == typeof(DirectCursorInputHandler))
            {
                return new DirectCursorInputHandler(context.GetService<CameraProvider>(),
                    context.GetService<CursorService>().cursorRigidbody);
            }
            else
            {
                return new JoystickInputHandler(context.GetService<Joystick>(), 
                    context.GetService<CursorService>().cursorRigidbody, joystickInputConfig);
            }
        }
    }
}