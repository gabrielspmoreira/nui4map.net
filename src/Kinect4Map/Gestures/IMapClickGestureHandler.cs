using System;
using MapUtils.Structs;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public interface IMapClickGestureHandler
    {
        object MapComponent { set; } 
        double MinZDistanceFromBody { get; set; }
        event Action<MapCoord> KinectMapClick;
        event Action<MapCoord> MouseMapClick;
        bool Detect(SkeletonPoint panHandPoint, SkeletonPoint otherHandPoint, SkeletonPoint headPoint);
    }
}