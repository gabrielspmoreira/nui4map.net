using System;
using ESRI.ArcGIS.Client;
using Kinect4EsriMap.Extensions;
using Kinect4Map.Gestures;
using Microsoft.Kinect;
using ESRI.ArcGIS.Client.Geometry;

namespace Kinect4EsriMap.Gestures
{
    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
    {
        #region Attributes

        private double _startDistance;
        private MapPoint _startRightHandCoordinate;
        private MapPoint _startLeftHandCoordinate;
        private double _startMapResolution;

        private Map _map;

        #endregion

        #region Events
        public override event Action KinectZoomStarted;
        public override event Action KinectZoomStopped;
        public override event Action KinectZooming;

        #endregion

        #region Properties

        public override object MapComponent
        {
            get { return _map; }
            set
            {
                if (value is Map)
                {
                    _map = value as Map;
                }
                else
                {
                    throw new InvalidCastException("Não é uma instância de Esri Map válida.");
                }
            }
        }

        #endregion

        #region Internal Methods

        protected override void StartZoom(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint)
        {
            IsZooming = true;
            _startMapResolution = _map.Resolution;
            _startRightHandCoordinate = rightHandPoint.ToEsriWebMercatorMapPoint(_map);
            _startLeftHandCoordinate = leftHandPoint.ToEsriWebMercatorMapPoint(_map);

            _startDistance = _startRightHandCoordinate.DistanceFrom(_startLeftHandCoordinate);

            if (KinectZoomStarted!= null)
            {
                KinectZoomStarted();
            }            
        }

        protected override void RunZooming(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint)
        {
            var rightHandCoordinate = rightHandPoint.ToEsriWebMercatorMapPoint(_map);
            var leftHandCoordinate = leftHandPoint.ToEsriWebMercatorMapPoint(_map);

            DoZoomMap(rightHandCoordinate, leftHandCoordinate);

            if (KinectZooming != null)
            {
                KinectZooming();
            }
        }

        protected override void StopZooming()
        {
            if (IsZooming)
            {
                IsZooming = false;
                if (KinectZoomStopped != null)
                {
                    KinectZoomStopped();
                }                
            }
        }


        private void DoZoomMap(MapPoint rightHandCoordinate, MapPoint leftHandCoordinate)
        {
            var centerX = (rightHandCoordinate.X + leftHandCoordinate.X) / 2;
            var centerY = (rightHandCoordinate.Y + leftHandCoordinate.Y) / 2;
            var zoomCenter = new MapPoint(centerX, centerY, _map.SpatialReference);

            var currentDistance = rightHandCoordinate.DistanceFrom(leftHandCoordinate);

            var zoomFactor = (currentDistance / _startDistance);
            var targetResolution = _startMapResolution / Math.Pow(zoomFactor, 2);

            _map.ZoomToResolution(targetResolution, zoomCenter);
        }


        #endregion
    }
}
