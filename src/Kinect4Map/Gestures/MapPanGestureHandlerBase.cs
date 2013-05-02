using System;
using MapUtils.Structs;
using Microsoft.Kinect;
using System.Linq;
using NUI4Map.Structs;
using NUI4Map.Gestures;
using Kinect4Map.KinectUtils;
using Kinect4Map.Extensions;

namespace Kinect4Map.Gestures
{
    public abstract class MapPanGestureHandlerBase : IMapPanGestureHandler
    {
        public double _minZDistanceFromBody;
        public bool IsPanning { get; protected set; }
        public Hand PanningHand { get; protected set; }
        public abstract object MapComponent { get; set; }
        public abstract event Action<MapCoord> PanStart;
        public abstract event Action PanStop;
        public abstract event Action<MapCoord> Panning;

        protected MapPanGestureHandlerBase()
        {
            _minZDistanceFromBody = 0.35;
        }

        public bool Detect(object frame)
        {
            var skeleton = (Skeleton)frame;

            var rightHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandRight);
            var rightHandTracked = rightHandJoint.TrackingState == JointTrackingState.Tracked;
            var rightHandPosition = rightHandJoint.Position;

            var leftHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandLeft);
            var leftHandTracked = leftHandJoint.TrackingState == JointTrackingState.Tracked;
            var leftHandPosition = leftHandJoint.Position;

            var shoulderCenterPosition =
                skeleton.Joints.First(j => j.JointType == JointType.ShoulderCenter).Position;

            // Calculate Skeleton Height (without head)
            var skeletonHeight = skeleton.Height();
            var minZDistanceFromBody = skeletonHeight / 4;

            var rightHandInFront = rightHandTracked && (shoulderCenterPosition.Z - rightHandPosition.Z >= _minZDistanceFromBody);
            var leftHandInFront = leftHandTracked && (shoulderCenterPosition.Z - leftHandPosition.Z >= _minZDistanceFromBody);

            // Just one hand at minimal distance from Shoulder Center
            if (rightHandInFront ^ leftHandInFront)
            {
                Vector3D handPoint;
                if (rightHandInFront)
                {
                    handPoint = rightHandPosition.ToVector3D();
                    PanningHand = Hand.Right;
                }
                else
                {
                    handPoint = leftHandPosition.ToVector3D();
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

        protected abstract void StartPan(Vector3D handPoint);
        protected abstract void RunPanning(Vector3D handPoint);
        protected abstract void StopZooming();
    }
}