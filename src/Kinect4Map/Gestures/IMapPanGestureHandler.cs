using System;
using MapUtils.Structs;
using Kinect4TelerikMap.Structs;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public interface IMapPanGestureHandler
    {
        object MapComponent { set; } 
        double MinZDistanceFromBody { get; set; }
        bool IsPanning { get; }
        Hand PanningHand { get; }
        event Action<MapCoord> KinectPanStart;
        event Action KinectPanStop;
        event Action<MapCoord> KinectPanning;

        bool Detect(SkeletonPoint shoulderCenter, SkeletonPoint rightHandPoint, bool rightHandTracked,
                                    SkeletonPoint leftHandPoint, bool leftHandTracked);
    }
}