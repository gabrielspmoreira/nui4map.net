using System;
using System.Diagnostics;
using Leap;
using MapUtils.Structs;
using Leap4Map.Extensions;
using NUI4Map.Structs;
using NUI4Map.Gestures;
using System.Linq;
using Hand = NUI4Map.Structs.Hand;

namespace Leap4Map.Gestures
{
    public abstract class MapPanGestureHandlerBase : IMapPanGestureHandler
    {
        public bool IsPanning { get; protected set; }
        public Hand PanningHand { get; protected set; }
        public abstract object MapComponent { get; set; }
        public event Action<MapCoord> PanStart;
        public event Action PanStop;
        public event Action<MapCoord> Panning;

        protected MapPanGestureHandlerBase()
        {

        }

        public bool Detect(object frame)
        {
            var leapFrame = (Leap.Frame)frame;

            // Get gestures
            var gestures = leapFrame.Gestures();
            
            if (gestures.Count > 0)
            {
                var gesture = gestures[0];

                switch (gesture.Type)
                {
                    case Gesture.GestureType.TYPESWIPE:
                        var swipe = new SwipeGesture(gesture);


                        if (swipe.State == Gesture.GestureState.STATESTART)
                        {
                            Log(swipe.State.ToString(), swipe.Position.ToVector3D(), swipe.Direction.ToVector3D(), swipe.Duration, swipe.Speed);

                            if (!IsPanning)
                            {
                                StartPan();
                            }

                            RunPanning(swipe.Direction.ToVector3D());
                        }
                        else if (swipe.State == Gesture.GestureState.STATESTOP)
                        {
                            StopPanning();
                        }

                        break;
                }
            }

            if (gestures.Count == 0)
            {
                if (IsPanning)
                {
                    StopPanning();
                }
            }

            return IsPanning;
        }

        private void Log(string prefix, Vector3D vector3D, Vector3D direction, long duration, float speed)
        {
            Debug.WriteLine(prefix + " - X: " + vector3D.X + " Y: " + vector3D.Y + " / X: " + direction.X + " Y: " + direction.Y + " Duration: "+duration.ToString()+" Speed: "+speed.ToString());
        }

        protected virtual void RunPanning(Vector3D movementVector)
        {
            DoPan(movementVector);

            if (Panning != null)
            {
                Panning(new MapCoord());
            }
        }

        protected virtual void StartPan()
        {
            IsPanning = true;

            if (PanStart != null)
            {
                PanStart(new MapCoord());
            }
        }

        protected virtual void StopPanning()
        {
            if (IsPanning)
            {
                IsPanning = false;

                if (PanStop != null)
                {
                    PanStop();
                }
            }
        }

        protected abstract void DoPan(Vector3D movementVector);
    }
}