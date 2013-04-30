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

        public bool Detect(Leap.Frame frame)
        {
            if (!frame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count == 5)
                {

                    if (!IsZooming)
                    {
                        StartZoom(hand);
                    }

                    RunZooming(hand);
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

        protected abstract void StartZoom(Leap.Hand hand);
        protected abstract void RunZooming(Leap.Hand hand);
        protected abstract void StopZooming();
    }
}