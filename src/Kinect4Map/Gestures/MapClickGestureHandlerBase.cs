using System;
using MapUtils.Structs;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public abstract class MapClickGestureHandlerBase : IMapClickGestureHandler
    {
        public abstract object MapComponent { get; set; }
        public double MinZDistanceFromBody { get; set; }
        public abstract event Action<MapCoord> KinectMapClick;
        public abstract event Action<MapCoord> MouseMapClick;

        public bool Detect(SkeletonPoint panHandPoint, SkeletonPoint otherHandPoint, SkeletonPoint headPoint)
        {
            if (otherHandPoint.Y > headPoint.Y)
            {
                DoMapClick(panHandPoint);
                return true;
            }
            return false;
        }

        protected abstract void DoMapClick(SkeletonPoint handPoint);
    }
}