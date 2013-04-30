using System;
using System.Windows;
using MapUtils.Distance;
using MapUtils.Structs;
using Leap;

namespace Leap4Map.Extensions
{
    public static class LeapMapExtensions
    {

        public static Point ToScreenPoint(this Leap.Vector v, double screenWidth, double screenHeight)
        {
            var x = Math.Min(100, Math.Max(0, v.x+50)) / 100;
            var y = Math.Min(75, Math.Max(0, v.y-75)) / 75;
            var screenX = (int)(x * screenWidth);
            var screenY = (int)(screenHeight - y * screenHeight);

            return new Point(screenX, screenY);
        }


        public static double DistanceFrom(this Leap.Vector point1, Leap.Vector point2)
        {
            return new Point(point1.x, point1.y).DistanceFrom(new Point(point2.x, point2.y));
        }

        public static Point DistanceVectorFrom(this Leap.Vector point1, Leap.Vector point2, double controlWidth, double controlHeight)
        {
            var screenPoint1 = point1.ToScreenPoint(controlWidth, controlHeight);
            var screenPoint2 = point2.ToScreenPoint(controlWidth, controlHeight);

            var deltaX = (screenPoint1.X - screenPoint2.X) / controlWidth;
            var deltaY = (screenPoint1.Y - screenPoint2.Y) / controlHeight;

            return new Point(deltaX, deltaY);
        }

    }
}
