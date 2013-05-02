using System;
using System.Windows;
using MapUtils.Distance;
using MapUtils.Structs;
using Leap;
using NUI4Map.Structs;

namespace Leap4Map.Extensions
{
    public static class LeapMapExtensions
    {

        public static Point ToScreenPoint(this Vector3D v, double screenWidth, double screenHeight)
        {
            var x = Math.Min(100, Math.Max(0, v.X+50)) / 100;
            var y = Math.Min(75, Math.Max(0, v.Y-75)) / 75;
            var screenX = (int)(x * screenWidth);
            var screenY = (int)(screenHeight - y * screenHeight);

            return new Point(screenX, screenY);
        }

        public static double DistanceFrom(this Vector3D point1, Vector3D point2)
        {
            return new Point(point1.X, point1.Y).DistanceFrom(new Point(point2.X, point2.Y));
        }

        public static Point DistanceVectorFrom(this Vector3D point1, Vector3D point2, double controlWidth, double controlHeight)
        {
            var screenPoint1 = point1.ToScreenPoint(controlWidth, controlHeight);
            var screenPoint2 = point2.ToScreenPoint(controlWidth, controlHeight);

            var deltaX = (screenPoint1.X - screenPoint2.X) / controlWidth;
            var deltaY = (screenPoint1.Y - screenPoint2.Y) / controlHeight;

            return new Point(deltaX, deltaY);
        }

        public static Vector3D ToVector3D(this Leap.Vector vector)
        {
            return new Vector3D
            {
                X = vector.x,
                Y = vector.y,
                Z = vector.z
            };
        }

    }
}
