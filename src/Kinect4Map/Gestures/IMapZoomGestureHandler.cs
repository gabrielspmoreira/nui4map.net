using System;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public interface IMapZoomGestureHandler
    {
        object MapComponent { set; } 
        event Action KinectZoomStarted;
        event Action KinectZoomStopped;
        event Action KinectZooming;
        double MinZDistanceFromBody { get; set; }
        bool IsZooming { get; }

        bool Detect(SkeletonPoint shoulderCenter, SkeletonPoint rightHandPoint, bool rightHandTracked,
                                    SkeletonPoint leftHandPoint, bool leftHandTracked);
    }
}