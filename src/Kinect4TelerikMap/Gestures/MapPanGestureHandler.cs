using System;
using System.Windows;
using Kinect4Map.Extensions;
using Kinect4Map.Gestures;
using Kinect4TelerikMap.Extensions;
using MapUtils.Structs;
using Microsoft.Kinect;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;

namespace Kinect4TelerikMap.Gestures
{

    public class MapPanGestureHandler : MapPanGestureHandlerBase
    {

        private RadMap _map;
        private Rect _startExtent;
        private Location _startHandCoordinate;
        private SkeletonPoint _startHandPoint;
        private Location _startMapCenter;

        public override event Action<MapCoord> KinectPanning;

        public override event Action<MapCoord> KinectPanStart;

        public override event Action KinectPanStop;     

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

        protected void DoPan(SkeletonPoint handPoint)
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

        protected override void RunPanning(SkeletonPoint handPoint)
        {
            var screenCoordinate = handPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            var location = Location.GetCoordinates(_map, screenCoordinate);
            DoPan(handPoint);

            if (KinectPanning != null)
            {
                KinectPanning(location.ToMapCoord());
            }
        }

        protected override void StartPan(SkeletonPoint handPoint)
        {
            IsPanning = true;
            _startHandPoint = handPoint;
            var screenCoordinate = handPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            _startHandCoordinate = Location.GetCoordinates(_map, screenCoordinate);

            var rect = _map.GeographicalBounds;

            _startExtent = new Rect(rect.TopLeft, rect.BottomRight);
            var currCenter = _map.Center; 
            _startMapCenter = new Location(currCenter.Latitude, currCenter.Longitude);

            if (KinectPanStart != null)
            {
                KinectPanStart(_startHandCoordinate.ToMapCoord());
            }
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

    } // class MapPanGestureHandler

}

