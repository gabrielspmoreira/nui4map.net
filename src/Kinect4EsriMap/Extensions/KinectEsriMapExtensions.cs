using System;
using System.Windows;
using ESRI.ArcGIS.Client.Projection;
using MapUtils.Converter;
using MapUtils.Distance;
using MapUtils.Structs;
using Microsoft.Kinect;
using ESRI.ArcGIS.Client.Geometry;
using ESRI.ArcGIS.Client;
using Kinect4Map.Extensions;
using NUI4Map.Structs;

namespace Kinect4EsriMap.Extensions
{
    public static class KinectEsriMapExtensions
    {

        public static MapPoint ToEsriWebMercatorMapPoint(this Vector3D handPoint, Map map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            var mapPoint = map.ScreenToMap(screenPoint);
            return mapPoint;
        }
    }
}
