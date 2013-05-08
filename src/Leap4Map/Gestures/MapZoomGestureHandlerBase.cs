using NUI4Map.Gestures;
using System;

namespace Leap4Map.Gestures
{
    public abstract class MapZoomGestureHandlerBase : IMapZoomGestureHandler
    {
        public abstract event Action ZoomStarted;
        public abstract event Action ZoomStopped;
        public abstract event Action Zooming;
        public bool IsZooming { get; protected set; }
        public abstract object MapComponent { get; set; }

        protected MapZoomGestureHandlerBase()
        {
            
        }

        public bool Detect(object frame)
        {
            var leapFrame = (Leap.Frame)frame;
            if (!leapFrame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = leapFrame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count >= 3 && hand.SphereRadius >= 60)
                {

                    if (!IsZooming)
                    {
                        StartZoom(leapFrame);
                    }

                    RunZooming(leapFrame);
                }
                else
                {
                    StopZooming();
                }
            }
            else
            {
                StopZooming();
            }
            return IsZooming;
        }

        protected abstract void StartZoom(Leap.Frame frame);
        protected abstract void RunZooming(Leap.Frame frame);
        protected abstract void StopZooming();
    }
}