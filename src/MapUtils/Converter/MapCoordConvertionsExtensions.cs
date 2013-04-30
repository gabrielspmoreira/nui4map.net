using System;
using MapUtils.Structs;

namespace MapUtils.Converter
{
    public static class MapCoordConvertionsExtensions
    {
        public static MapCoord ToGeographic(this MapCoord mapCoord)
        {
            if (mapCoord.ProjectionWkid == CoordinateSystems.Wgs84ProjectionWkid)
            {
                return mapCoord;
            }

            var mercatorXLon = mapCoord.Longitude;
            var mercatorYLat = mapCoord.Latitude;

            if (Math.Abs(mercatorXLon) < 180 && Math.Abs(mercatorYLat) < 90)
            {
                throw new IndexOutOfRangeException("It appears to be Geographic coordinates");
            }

            if ((Math.Abs(mercatorXLon) > 20037508.3427892) || (Math.Abs(mercatorYLat) > 20037508.3427892))
            {
                throw new IndexOutOfRangeException("WebMercator coordinates out of bounds");
            }

            var x = mercatorXLon;
            var y = mercatorYLat;
            var num3 = x / 6378137.0;
            var num4 = num3 * 57.295779513082323;
            var num5 = Math.Floor(((num4 + 180.0) / 360.0));
            var num6 = num4 - (num5 * 360.0);
            var num7 = 1.5707963267948966 - (2.0 * Math.Atan(Math.Exp((-1.0 * y) / 6378137.0)));
            mercatorXLon = num6;
            mercatorYLat = num7 * 57.295779513082323;

            var reprojPoint = new MapCoord
                                  {
                                      Latitude = mercatorYLat,
                                      Longitude = mercatorXLon,
                                      ProjectionWkid = CoordinateSystems.Wgs84ProjectionWkid
                                  };
            return reprojPoint;
        }

        public static MapCoord ToWebMercator(this MapCoord mapCoord)
        {
            if (mapCoord.ProjectionWkid == CoordinateSystems.WebMercatorProjectionWkid)
            {
                return mapCoord;
            }

            var wgs84XLon = mapCoord.Longitude;
            var wgs84YLat = mapCoord.Latitude;

            if ((Math.Abs(wgs84XLon) > 180 || Math.Abs(wgs84YLat) > 90))
            {
                throw new IndexOutOfRangeException("WGS84 coordinates out of bounds");
            }

            var num = wgs84XLon * 0.017453292519943295;
            var x = 6378137.0 * num;
            var a = wgs84YLat * 0.017453292519943295;

            wgs84XLon = x;
            wgs84YLat = 3189068.5 * Math.Log((1.0 + Math.Sin(a)) / (1.0 - Math.Sin(a)));

            var reprojPoint = new MapCoord { Latitude = wgs84YLat, 
                                            Longitude = wgs84XLon,
                                            ProjectionWkid = CoordinateSystems.WebMercatorProjectionWkid
                                            };
            return reprojPoint;
        }
    }
}
