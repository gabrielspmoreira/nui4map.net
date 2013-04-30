using System;
using System.Windows;
using Kinect4Map.Extensions;
using Kinect4Map.Gestures;
using MapUtils.Distance;
using Microsoft.Kinect;
using Telerik.Windows.Controls;

namespace Kinect4TelerikMap.Gestures
{

    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
    {

        private RadMap _map;
        private double _startDistance;
        private Point _startLeftHandScreenCoord;
        private Point _startRightHandScreenCoord;
        private int _startZoomLevel;
        private const float _PixelsDistanteToChangeZoomLevel = 400;

        public override event Action KinectZooming;
        public override event Action KinectZoomStarted;
        public override event Action KinectZoomStopped;

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
                    throw new InvalidCastException("Não é uma instância de RadMap válida");
                }
            }
        }


        private void DoZoomMap(Point rightHandCoordinate, Point leftHandCoordinate)
        {
            var distance = rightHandCoordinate.DistanceFrom(leftHandCoordinate);
            
            var levelDiff = (int)((distance - _startDistance) / _PixelsDistanteToChangeZoomLevel);
            _map.ZoomLevel = _startZoomLevel + levelDiff;
        }

        protected override void RunZooming(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint)
        {
            var rightHandCoordinate = rightHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            var leftHandCoordinate = leftHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            DoZoomMap(rightHandCoordinate, leftHandCoordinate);
            
            if (KinectZooming != null)
            {
                KinectZooming();
            }
        }

        protected override void StartZoom(SkeletonPoint rightHandPoint, SkeletonPoint leftHandPoint)
        {
            IsZooming = true;
            _startRightHandScreenCoord = rightHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            _startLeftHandScreenCoord = leftHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            _startDistance = _startRightHandScreenCoord.DistanceFrom(_startLeftHandScreenCoord);
            _startZoomLevel = _map.ZoomLevel;
           
            if (KinectZoomStarted != null)
            {
                KinectZoomStarted();
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

    }

}

