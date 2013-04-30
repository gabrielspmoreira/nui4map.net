using System;
using Leap4TelerikMap.Structs;
using MapUtils.Structs;
using Leap;

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

        public bool Detect(Leap.Frame frame)
        {
            if (!frame.Hands.Empty)
            {
                // Get the first hand
                Leap.Hand hand = frame.Hands[0];

                // Check if the hand has any fingers
                var fingers = hand.Fingers;
                if (!fingers.Empty && fingers.Count == 1 && fingers[0].TipPosition.z < 0)
                {
                    Leap.Vector handPoint = fingers[0].TipPosition;

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

        protected abstract void StartPan(Leap.Vector handPoint);
        protected abstract void RunPanning(Leap.Vector handPoint);
        protected abstract void StopPanning();
    }
}