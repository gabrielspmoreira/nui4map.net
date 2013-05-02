using System;
using MapUtils.Structs;
using NUI4Map.Structs;
using Leap4Map.Extensions;
using NUI4Map.Gestures;

namespace Leap4Map.Gestures
{
    public abstract class MapClickGestureHandlerBase : IMapClickGestureHandler
    {
        public abstract object MapComponent { get; set; }
        public abstract event Action<MapCoord> NUIMapClick;
        public abstract event Action<MapCoord> MouseMapClick;

        public bool Detect(object frame)
        {
            var leapFrame = (Leap.Frame)frame;
            if (!leapFrame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = leapFrame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count == 2)
                {
                    var point = fingers[0].TipPosition.ToVector3D();
                    DoMapClick(point);
                    return true;
                }
            }
            return false;
        }

        protected abstract void DoMapClick(Vector3D handPoint);
    }
}