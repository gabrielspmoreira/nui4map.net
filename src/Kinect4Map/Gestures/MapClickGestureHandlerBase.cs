using System;
using MapUtils.Structs;
using Microsoft.Kinect;
using NUI4Map.Gestures;
using System.Linq;
using NUI4Map.Structs;
using Kinect4Map.Extensions;

namespace Kinect4Map.Gestures
{
    public abstract class MapClickGestureHandlerBase : IMapClickGestureHandler
    {
        public abstract object MapComponent { get; set; }
        public double MinZDistanceFromBody { get; set; }
        public abstract event Action<MapCoord> NUIMapClick;
        public abstract event Action<MapCoord> MouseMapClick;

        public bool Detect(object frame)
        {
            var skeleton = (Skeleton)frame;

            var rightHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandRight);
            var rightHandTracked = rightHandJoint.TrackingState == JointTrackingState.Tracked;
            var rightHandPosition = rightHandJoint.Position;

            var leftHandJoint = skeleton.Joints.First(j => j.JointType == JointType.HandLeft);
            var leftHandTracked = leftHandJoint.TrackingState == JointTrackingState.Tracked;
            var leftHandPosition = leftHandJoint.Position;

            Hand handClicking;
            if (leftHandPosition.Z < rightHandPosition.Z)
            {
                handClicking = Hand.Left;
            }
            else
            {
                handClicking = Hand.Right;
            }

            var shoulderCenterPosition =
                skeleton.Joints.First(j => j.JointType == JointType.ShoulderCenter).Position;
            var headPosition = skeleton.Joints.First(j => j.JointType == JointType.Head).Position;

            if ((handClicking == Hand.Left && rightHandPosition.Y > headPosition.Y) ||
                (handClicking == Hand.Right && leftHandPosition.Y > headPosition.Y))
            {
                var handClicked = handClicking == Hand.Left ? leftHandPosition : rightHandPosition;
                DoMapClick(handClicked.ToVector3D());
                return true;
            }
            return false;
        }

        protected abstract void DoMapClick(Vector3D handPoint);
    }
}