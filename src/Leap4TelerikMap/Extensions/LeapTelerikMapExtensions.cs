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

        public static Location ToTelerikMapLocation(this Vector3D handPoint, RadMap map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            return Location.GetCoordinates(map, screenPoint);
        }
    } 

}

