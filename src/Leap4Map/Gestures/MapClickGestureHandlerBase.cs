using System;
using MapUtils.Structs;

namespace Leap4Map.Gestures
{
    public abstract class MapClickGestureHandlerBase : IMapClickGestureHandler
    {
        public abstract object MapComponent { get; set; }
        public abstract event Action<MapCoord> LeapMapClick;
        public abstract event Action<MapCoord> MouseMapClick;

        public bool Detect(Leap.Frame frame)
        {
            if (!frame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count == 2)
                {
                    Leap.Vector point = fingers[0].TipPosition;
                    DoMapClick(point);
                    return true;
                }
            }
            return false;
        }

        protected abstract void DoMapClick(Leap.Vector handPoint);
    }
}