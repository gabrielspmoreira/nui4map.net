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

namespace Kinect4EsriMap.Extensions
{
    public static class EsriMapExtensions
    {
        public static MapPoint ToEsriWebMercatorMapPoint(this MapCoord mapLocation)
        {
            if (mapLocation.ProjectionWkid == CoordinateSystems.WebMercatorProjectionWkid)
            {
                var esriMapPoint = new MapPoint(mapLocation.Longitude, mapLocation.Latitude, new SpatialReference(CoordinateSystems.WebMercatorProjectionWkid));
                return esriMapPoint;
            }
            else
            {
                if (mapLocation.ProjectionWkid != CoordinateSystems.Wgs84ProjectionWkid)
                {
                    throw new NotSupportedException(
                        String.Format(
                            "The WKID {0} is not supported.\nThe only supported coordinate systemas are WebMercator(102100) and WGS84 (4326))",
                           mapLocation.ProjectionWkid));
                }

                var esriMapPoint = new MapPoint(mapLocation.Longitude, mapLocation.Latitude, new SpatialReference(CoordinateSystems.Wgs84ProjectionWkid));
                // Project from WGS84 to WebMercator
                var reprojMapPoint = new WebMercator().FromGeographic(esriMapPoint) as MapPoint;
                return reprojMapPoint;
            }
        }

        public static MapPoint ToEsriWebMercatorMapPoint(this SkeletonPoint handPoint, Map map)
        {
            var screenPoint = handPoint.ToScreenPoint(map.ActualWidth, map.ActualHeight);
            var mapPoint = map.ScreenToMap(screenPoint);
            return mapPoint;
        }

        public static MapCoord ToMapCoord(this MapPoint mapPoint)
        {
            return new MapCoord { Latitude = mapPoint.Y, 
                                  Longitude = mapPoint.X,
                                  ProjectionWkid = CoordinateSystems.WebMercatorProjectionWkid 
                                };
        }           

        public static double DistanceFrom(this MapPoint point1, MapPoint point2)
        {
            return new Point(point1.X, point1.Y).DistanceFrom(new Point(point2.X, point2.Y));
        }

    }
}
