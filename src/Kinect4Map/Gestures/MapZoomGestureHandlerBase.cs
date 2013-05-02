using System;
using Microsoft.Kinect;
using NUI4Map.Gestures;
using System.Linq;
using Kinect4Map.KinectUtils;
using NUI4Map.Structs;
using Kinect4Map.Extensions;

namespace Kinect4Map.Gestures
{
    public abstract class MapZoomGestureHandlerBase : IMapZoomGestureHandler
    {
        public abstract event Action ZoomStarted;
        public abstract event Action ZoomStopped;
        public abstract event Action Zooming;
        public double _minZDistanceFromBody;
        public bool IsZooming { get; protected set; }
        public abstract object MapComponent { get; set; }

        protected MapZoomGestureHandlerBase()
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

            if (rightHandTracked == false || leftHandTracked == false)
            {
                StopZooming();
            }
                // Both hands at minimal distance from Shoulder Center
            else if (shoulderCenterPosition.Z - rightHandPosition.Z >= _minZDistanceFromBody &&
                     shoulderCenterPosition.Z - leftHandPosition.Z >= _minZDistanceFromBody)
            {
                if (!IsZooming)
                {
                    StartZoom(rightHandPosition.ToVector3D(), leftHandPosition.ToVector3D());
                }

                RunZooming(rightHandPosition.ToVector3D(), leftHandPosition.ToVector3D());
            }
            else
            {
                StopZooming();
            }

            return IsZooming;
        }

        protected abstract void StartZoom(Vector3D rightHandPoint, Vector3D leftHandPoint);
        protected abstract void RunZooming(Vector3D rightHandPoint, Vector3D leftHandPoint);
        protected abstract void StopZooming();
    }
}