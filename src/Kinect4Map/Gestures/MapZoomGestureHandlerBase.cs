using System;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public abstract class MapZoomGestureHandlerBase : IMapZoomGestureHandler
    {
        public abstract event Action KinectZoomStarted;
        public abstract event Action KinectZoomStopped;
        public abstract event Action KinectZooming;
        public double MinZDistanceFromBody { get; set; }
        public bool IsZooming { get; protected set; }
        public abstract object MapComponent { get; set; }

        protected MapZoomGestureHandlerBase()
        {
            MinZDistanceFromBody = 0.35;
        }

        public bool Detect(SkeletonPoint shoulderCenter, SkeletonPoint rightHandPoint, bool rightHandTracked,
                           SkeletonPoint leftHandPoint, bool leftHandTracked)
        {
            if (rightHandTracked == false || leftHandTracked == false)
            {
                StopZooming();
            }
                // Both hands at minimal distance from Shoulder Center
            else if (shoulderCenter.Z - rightHandPoint.Z >= MinZDistanceFromBody &&
                     shoulderCenter.Z - leftHandPoint.Z >= MinZDistanceFromBody)
            {
                if (!IsZooming)
                {
                    StartZoom(rightHandPoint, leftHandPoint);
                }

                RunZooming(rightHandPoint, leftHandPoint);
            }
            else
            {
                StopZooming();
            }

            return IsZooming;
        }

        protected abstract void StartZoom(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint);
        protected abstract void RunZooming(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint);
        protected abstract void StopZooming();
    }
}