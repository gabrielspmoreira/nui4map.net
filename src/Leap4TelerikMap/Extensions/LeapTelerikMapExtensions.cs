using System.Windows;
using Leap4Map.Extensions;
using MapUtils.Converter;
using MapUtils.Distance;
using MapUtils.Structs;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;
using NUI4Map.Structs;

namespace Leap4TelerikMap.Extensions
{
    public static class LeapTelerikMapExtensions
    {

        public static double DistanceFrom(this Location point1, Location point2)
        {
            return new Point(point1.Longitude, point1.Latitude).DistanceFrom(new Point(point2.Longitude, point2.Latitude));
        }

        public static MapCoord ToMapCoord(this Location mapPoint)
        {
            var mapCoord = new MapCoord {Latitude = mapPoint.Latitude, 
                                         Longitude = mapPoint.Longitude,
                                         ProjectionWkid = CoordinateSystems.Wgs84ProjectionWkid};
            return mapCoord;
        }

        public static Location ToTelerikMapLocation(this Vector3D handPoint, RadMap map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            return Location.GetCoordinates(map, screenPoint);
        }
    } 

}

