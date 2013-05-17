using System;
using System.Linq;
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
                var hand = leapFrame.Hands[0];

                // Check if the user is making an "L" with thumb and indicator fingers 
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count == 2 && 
                    fingers[0].TipPosition.DistanceTo(fingers[1].TipPosition) > 60 &&
                    Math.Abs(fingers[0].TipPosition.z - fingers[1].TipPosition.z) > 100)
                {
                    var minY = fingers.Max(f1 => f1.TipPosition.y);
                    var handPoint = fingers.First(f => f.TipPosition.y <= minY).TipPosition.ToVector3D();
                    DoMapClick(handPoint);
                    return true;
                }
            }
            return false;
        }

        protected abstract void DoMapClick(Vector3D handPoint);
    }
}