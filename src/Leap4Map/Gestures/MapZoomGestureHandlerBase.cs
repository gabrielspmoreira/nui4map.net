using NUI4Map.Gestures;
using System;
using Leap;
using System.Diagnostics;

namespace Leap4Map.Gestures
{
    public abstract class MapZoomGestureHandlerBase : IMapZoomGestureHandler
    {
        public event Action ZoomStarted;
        public event Action ZoomStopped;
        public event Action Zooming;

        private const long MIN_TIME_BETWEEN_ZOOM_GESTURES = 1000;

        protected Leap.Frame StartFrame;
        protected DateTime LastZoomTime;
        public bool IsZooming { get; protected set; }
        public abstract object MapComponent { get; set; }

        protected MapZoomGestureHandlerBase()
        {
            LastZoomTime = DateTime.Now;
        }

        public bool Detect(object frame)
        {
            var leapFrame = (Frame)frame;
            if (!leapFrame.Hands.Empty)
            {
                // Get the first hand
                var hand = leapFrame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;

                Debug.WriteLine(hand.PalmVelocity.Magnitude);
                if (!fingers.Empty && fingers.Count >= 3 && hand.PalmVelocity.Magnitude < 500)
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

        protected virtual void StartZoom(Leap.Frame frame)
        {
            IsZooming = true;
            StartFrame = frame;

            if (ZoomStarted != null)
            {
                ZoomStarted();
            }
        }

        protected virtual void StopZooming()
        {
            if (IsZooming)
            {
                IsZooming = false;

                if (ZoomStopped != null)
                {
                    ZoomStopped();
                }
            }
        }

        protected virtual void RunZooming(Leap.Frame frame)
        {
            // To avoid the effect of return to previous zoom when returning fingers to previous position
            if ((DateTime.Now - LastZoomTime).TotalMilliseconds <= MIN_TIME_BETWEEN_ZOOM_GESTURES)
               return;

            /*if (frame.ScaleProbability(StartFrame) < 0.8)
                return;
            */
            if (DoZoomMap(frame))
            {
                StartFrame = frame;
                LastZoomTime = DateTime.Now;
            }
           
            if (Zooming != null)
            {
                Zooming();
            }
        }

        protected abstract bool DoZoomMap(Leap.Frame frame);
    }
}