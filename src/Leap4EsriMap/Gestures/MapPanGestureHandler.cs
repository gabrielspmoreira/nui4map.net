using System;
using ESRI.ArcGIS.Client;
using EsriMapCommons.Extensions;
using Leap4EsriMap.Extensions;
using Leap4Map.Extensions;
using Leap4Map.Gestures;
using MapUtils.Structs;
using ESRI.ArcGIS.Client.Geometry;
using NUI4Map.Structs;

namespace Leap4EsriMap.Gestures
{
    public class MapPanGestureHandler : MapPanGestureHandlerBase
    {
        #region Attributes

        private Map _map;

        private Envelope _startExtent;
        private MapPoint _startHandCoordinate;
        private Vector3D _startHandPoint;

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
        public override event Action<MapCoord> PanStart;
        public override event Action PanStop;
        public override event Action<MapCoord> Panning;
        #endregion

        #region Private Methods

        protected override void StartPan(Vector3D handPoint)
        {
            IsPanning = true;
            _startHandPoint = handPoint;
            _startHandCoordinate = handPoint.ToEsriWebMercatorMapPoint(_map);
            _startExtent = new Envelope(_map.Extent.XMin, _map.Extent.YMin, _map.Extent.XMax, _map.Extent.YMax);

            if (PanStart != null)
            {
                PanStart(_startHandCoordinate.ToMapCoord());
            }
        }

        protected override void RunPanning(Vector3D handPoint)
        {
            DoPan(handPoint);
        }

        protected override void StopPanning()
        {
            if (IsPanning)
            {
                IsPanning = false;

                if (PanStop != null)
                {
                    PanStop();
                }
            }
        }

        protected void DoPan(Vector3D handPoint)
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

            if (Panning != null)
            {
                var handCoordinate = handPoint.ToEsriWebMercatorMapPoint(_map);
                Panning(handCoordinate.ToMapCoord());
            }
        }

        #endregion
    }
}
