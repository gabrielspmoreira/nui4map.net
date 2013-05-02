using System;
using System.Windows;
using MapUtils.Distance;
using MapUtils.Structs;
using Microsoft.Kinect;
using NUI4Map.Structs;

namespace Kinect4Map.Extensions
{
    public static class MapExtensions
    {
        private const double _SpeedFactor = 1.5; // Factor to allow a faster and easier hands movement

        public static Point ToScreenPoint(this Vector3D skelpoint, double controlWidth, double controlHeight)
        {
            var scaledPoint = skelpoint.ScaleTo(controlWidth, controlHeight);

            // Making hands movement faster and easier
            var pointWithSpeedFactorX = scaledPoint.X * _SpeedFactor;
            var pointWithSpeedFactorY = scaledPoint.Y * _SpeedFactor;

            // Centering hands coordinates on map control size
            var adjCenteredX = pointWithSpeedFactorX - (((controlWidth * _SpeedFactor) - controlWidth) / 2);
            var adjCenteredY = pointWithSpeedFactorY - (((controlHeight * _SpeedFactor) - controlHeight) / 2);

            return new Point(adjCenteredX, adjCenteredY);
        }

        public static Point DistanceVectorFrom(this Vector3D point1, Vector3D point2, double controlWidth, double controlHeight)
        {
            var screenPoint1 = point1.ToScreenPoint(controlWidth, controlHeight);
            var screenPoint2 = point2.ToScreenPoint(controlWidth, controlHeight);

            var deltaX = (screenPoint1.X - screenPoint2.X) / controlWidth;
            var deltaY = (screenPoint1.Y - screenPoint2.Y) / controlHeight;

            return new Point(deltaX, deltaY);
        }

        public static double DistanceFrom(this Vector3D point1, Vector3D point2)
        {
            return new Point(point1.X, point1.Y).DistanceFrom(new Point(point2.X, point2.Y));
        }

        public static Vector3D ToVector3D(this SkeletonPoint vector)
        {
            return new Vector3D
            {
                X = vector.X,
                Y = vector.Y,
                Z = vector.Z
            };
        }
    }
}
