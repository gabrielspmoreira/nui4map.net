using System;
using System.Windows;
using Leap4TelerikMap.Extensions;
using MapUtils.Structs;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;
using Leap4Map.Gestures;
using Leap4Map.Extensions;
using NUI4Map.Structs;
using TelerikMapCommons.Extensions;

namespace Leap4TelerikMap.Gestures
{

    public class MapPanGestureHandler : MapPanGestureHandlerBase
    {

        private RadMap _map;
        private Rect _startExtent;
        private Location _startHandCoordinate;
        private Vector3D _startHandPoint;
        private Location _startMapCenter;

        public override event Action<MapCoord> Panning;

        public override event Action<MapCoord> PanStart;

        public override event Action PanStop;     

        public override object MapComponent
        {
            get
            {
                return _map;
            }
            set
            {
                if (value is RadMap)
                {
                    _map = value as RadMap;
                }
                else
                {
                    throw new InvalidCastException("Não é uma instância de RadMap válida.");
                }                    
            }
        }

        protected void DoPan(Vector3D handPoint)
        {
            var relativeDeltaDistance = _startHandPoint.DistanceVectorFrom(handPoint, _map.ActualWidth, _map.ActualHeight);
            var deltaX = relativeDeltaDistance.X * _startExtent.Width;
            var deltaY = relativeDeltaDistance.Y * _startExtent.Height;

            var nextCenter = new Location
                               {
                                   Longitude = _startMapCenter.Longitude + deltaX,
                                   Latitude = _startMapCenter.Latitude - deltaY
                               };
            _map.Center = nextCenter;
        }

        protected override void RunPanning(Vector3D handPoint)
        {
            var screenCoordinate = handPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            var location = Location.GetCoordinates(_map, screenCoordinate);
            DoPan(handPoint);

            if (Panning != null)
            {
                Panning(location.ToMapCoord());
            }
        }

        protected override void StartPan(Vector3D handPoint)
        {
            IsPanning = true;
            _startHandPoint = handPoint;
            var screenCoordinate = handPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            _startHandCoordinate = Location.GetCoordinates(_map, screenCoordinate);

            var rect = _map.GeographicalBounds;

            _startExtent = new Rect(rect.TopLeft, rect.BottomRight);
            var currCenter = _map.Center; 
            _startMapCenter = new Location(currCenter.Latitude, currCenter.Longitude);

            if (PanStart != null)
            {
                PanStart(_startHandCoordinate.ToMapCoord());
            }
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

    } // class MapPanGestureHandler

}

