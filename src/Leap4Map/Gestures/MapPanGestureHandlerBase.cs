using System;
using MapUtils.Structs;
using Leap;
using Leap4Map.Extensions;
using NUI4Map.Structs;
using NUI4Map.Gestures;

namespace Leap4Map.Gestures
{
    public abstract class MapPanGestureHandlerBase : IMapPanGestureHandler
    {
        public bool IsPanning { get; protected set; }
        public abstract object MapComponent { get; set; }
        public abstract event Action<MapCoord> PanStart;
        public abstract event Action PanStop;
        public abstract event Action<MapCoord> Panning;

        protected MapPanGestureHandlerBase()
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
                if (!fingers.Empty && fingers.Count == 1 && fingers[0].TipPosition.z < 0)
                {
                    var handPoint = fingers[0].TipPosition.ToVector3D();

                    if (!IsPanning)
                    {
                        StartPan(handPoint);
                    }

                    RunPanning(handPoint);
                }
                else
                {
                    StopPanning();
                }
            }

            return IsPanning;
        }

        protected abstract void StartPan(Vector3D handPoint);
        protected abstract void RunPanning(Vector3D handPoint);
        protected abstract void StopPanning();
    }
}