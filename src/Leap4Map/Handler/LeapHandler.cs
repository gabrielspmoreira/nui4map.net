using System;
using System.Linq;
using System.Windows;
using Leap;
using NUI4Map.Handler;
using NUI4Map.Structs;

namespace Leap4Map.Handler
{
    public class LeapHandler : INUIHandler
    {
        #region Fields

        public SensorType SensorType { get; private set; }
        protected Controller controller;
        protected LeapListener listener;

        #endregion

        #region Events
        public event Action<object> OnConnect;
        public event Action<object> OnFrame;

        #endregion

        public LeapHandler()
        {
            SensorType = SensorType.Leap;
        }

        public void Start()
        {
            // Create a sample listener and controller
            listener = new LeapListener();
            controller = new Controller();

            // Have the sample listener receive events from the controller
            controller.AddListener(listener);

            listener.FrameEvent += listener_FrameEvent;
            listener.ConnectEvent += listener_ConnectEvent;
        }

        void listener_ConnectEvent(Controller obj)
        {
            // Enabling Swipe Gesture detection for pan
            controller.EnableGesture(Gesture.GestureType.TYPESWIPE);

            if (OnConnect != null)
            {
                OnConnect(obj);
            }
        }

        void listener_FrameEvent(Controller obj)
        {
            if (OnFrame != null)
            {
                OnFrame(obj);
            }
        }

        public void Stop()
        {
            if (controller != null)
            {
                // Remove the sample listener when done
                controller.RemoveListener(listener);
                controller.Dispose();
                controller = null;
            }
        }
    }

}
