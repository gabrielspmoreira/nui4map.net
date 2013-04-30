using System;
using Kinect4TelerikMap.Structs;
using MapUtils.Structs;
using Microsoft.Kinect;

namespace Kinect4Map.Gestures
{
    public abstract class MapPanGestureHandlerBase : IMapPanGestureHandler
    {
        public double MinZDistanceFromBody { get; set; }
        public bool IsPanning { get; protected set; }
        public Hand PanningHand { get; protected set; }
        public abstract object MapComponent { get; set; }
        public abstract event Action<MapCoord> KinectPanStart;
        public abstract event Action KinectPanStop;
        public abstract event Action<MapCoord> KinectPanning;

        protected MapPanGestureHandlerBase()
        {
            MinZDistanceFromBody = 0.35;
        }

        public bool Detect(SkeletonPoint shoulderCenter, SkeletonPoint rightHandPoint, bool rightHandTracked,
                           SkeletonPoint leftHandPoint, bool leftHandTracked)
        {

            var rightHandInFront = rightHandTracked && (shoulderCenter.Z - rightHandPoint.Z >= MinZDistanceFromBody);
            var leftHandInFront = leftHandTracked && (shoulderCenter.Z - leftHandPoint.Z >= MinZDistanceFromBody);

            // Just one hand at minimal distance from Shoulder Center
            if (rightHandInFront ^ leftHandInFront)
            {
                SkeletonPoint handPoint;
                if (rightHandInFront)
                {
                    handPoint = rightHandPoint;
                    PanningHand = Hand.Right;
                }
                else
                {
                    handPoint = leftHandPoint;
                    PanningHand = Hand.Left;
                }

                if (!IsPanning)
                {
                    StartPan(handPoint);
                }

                RunPanning(handPoint);
            }
            else
            {
                StopZooming();
            }

            return IsPanning;
        }

        protected abstract void StartPan(SkeletonPoint handPoint);
        protected abstract void RunPanning(SkeletonPoint handPoint);
        protected abstract void StopZooming();
    }
}