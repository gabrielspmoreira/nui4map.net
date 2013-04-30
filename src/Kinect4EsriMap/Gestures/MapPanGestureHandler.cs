using System;
using ESRI.ArcGIS.Client;
using Kinect4EsriMap.Extensions;
using Kinect4Map.Extensions;
using Kinect4Map.Gestures;
using MapUtils.Structs;
using Microsoft.Kinect;
using ESRI.ArcGIS.Client.Geometry;

namespace Kinect4EsriMap.Gestures
{
    public class MapPanGestureHandler : MapPanGestureHandlerBase
    {
        #region Attributes

        private Map _map;

        private Envelope _startExtent;
        private MapPoint _startHandCoordinate;
        private SkeletonPoint _startHandPoint;

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

        #region Events
        public override event Action<MapCoord> KinectPanStart;
        public override event Action KinectPanStop;
        public override event Action<MapCoord> KinectPanning;
        #endregion

        #region Private Methods

        protected override void StartPan(SkeletonPoint handPoint)
        {
            IsPanning = true;
            _startHandPoint = handPoint;
            _startHandCoordinate = handPoint.ToEsriWebMercatorMapPoint(_map);
            _startExtent = new Envelope(_map.Extent.XMin, _map.Extent.YMin, _map.Extent.XMax, _map.Extent.YMax);

            if (KinectPanStart != null)
            {
                KinectPanStart(_startHandCoordinate.ToMapCoord());
            }
        }

        protected override void RunPanning(SkeletonPoint handPoint)
        {
            DoPan(handPoint);      
        }

        protected override void StopZooming()
        {
            if (IsPanning)
            {
                IsPanning = false;

                if (KinectPanStop != null)
                {
                    KinectPanStop();
                }                
            }
        }

        protected void DoPan(SkeletonPoint handPoint)
        {
            var mapExtentDeltaX = (_startExtent.XMax - _startExtent.XMin);
            var mapExtentDeltaY = (_startExtent.YMax - _startExtent.YMin);

            var relativeDeltaDistance = _startHandPoint.DistanceVectorFrom(handPoint, _map.ActualWidth, _map.ActualHeight);
            var deltaX = relativeDeltaDistance.X * mapExtentDeltaX;
            var deltaY = relativeDeltaDistance.Y * mapExtentDeltaY;

            var nextExtent = new Envelope
                                 {
                                     XMin = _startExtent.XMin + deltaX,
                                     XMax = _startExtent.XMax + deltaX,
                                     YMin = _startExtent.YMin - deltaY,
                                     YMax = _startExtent.YMax - deltaY
                                 };

            _map.Extent = nextExtent;

            if (KinectPanning != null)
            {
                var handCoordinate = handPoint.ToEsriWebMercatorMapPoint(_map);
                KinectPanning(handCoordinate.ToMapCoord());           
            }
        }

        #endregion
    }
}
