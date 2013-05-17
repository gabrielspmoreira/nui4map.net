using Kinect4Map.Extensions;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;
using NUI4Map.Structs;

namespace Kinect4TelerikMap.Extensions
{
    public static class KinectTelerikMapExtensions
    {

        public static Location ToTelerikMapLocation(this Vector3D handPoint, RadMap map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            return Location.GetCoordinates(map, screenPoint);
        }
    } 

}

