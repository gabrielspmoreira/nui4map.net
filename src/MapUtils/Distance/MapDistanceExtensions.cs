using System;
using System.Windows;
using MapUtils.Converter;
using MapUtils.Structs;

namespace MapUtils.Distance
{
    public static class MapDistanceExtensions
    {
        public static double KmDistanceFrom(this MapCoord wgs84Coords1, MapCoord wgs84Coords2)
        {
            var location1Reproj = wgs84Coords1.ToWebMercator();
            var location2Reproj = wgs84Coords2.ToWebMercator();

            var meterDistance = location1Reproj.ToPoint().DistanceFrom(location2Reproj.ToPoint());
            var kmDistance = meterDistance/1000;
            return kmDistance;
        }

        public static double DistanceFrom(this Point point1, Point point2)
        {
            // Calculating distance through Pitagoras Theorem
            return Math.Sqrt(Math.Pow((point1.X - point2.X), 2) +
                             Math.Pow((point1.Y - point2.Y), 2));
        }

        private static Point ToPoint(this MapCoord coord)
        {
            return new Point(coord.Longitude, coord.Latitude);
        }
    }
}
