using Leap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Leap4Map.Handler
{
    public class LeapListener : Listener
    {

        public event Action<Controller> FrameEvent;
        public event Action<Controller> ConnectEvent;
        public event Action<Controller> DisconnectEvent;
        public event Action<Controller> InitEvent;
        public event Action<Controller> ExitEvent;


        public override void OnInit(Controller controller)
        {
            if (InitEvent != null)
            {
                InitEvent(controller);
            }
        }

        public override void OnConnect(Controller controller)
        {
            if (ConnectEvent != null)
            {
                ConnectEvent(controller);
            }
            
            /*controller.EnableGesture(Gesture.GestureType.TYPECIRCLE);
            controller.EnableGesture(Gesture.GestureType.TYPEKEYTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESCREENTAP);
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);*/
        }

        public override void OnDisconnect(Controller controller)
        {
            if (DisconnectEvent != null)
            {
                DisconnectEvent(controller);
            }
            
        }

        public override void OnExit(Controller controller)
        {
            if (ExitEvent != null)
            {
                ExitEvent(controller);
            }
            
        }

        public override void OnFrame(Controller controller)
        {
            if (FrameEvent != null)
            {
                FrameEvent(controller);
            }
        }
    }
}
