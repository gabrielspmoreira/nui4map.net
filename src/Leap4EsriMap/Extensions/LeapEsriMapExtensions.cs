using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using Leap4Map.Extensions;
using NUI4Map.Structs;

namespace Leap4EsriMap.Extensions
{
    public static class LeapEsriMapExtensions
    {

        public static MapPoint ToEsriWebMercatorMapPoint(this Vector3D handPoint, Map map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            var mapPoint = map.ScreenToMap(screenPoint);
            return mapPoint;
        }
    } 

}

