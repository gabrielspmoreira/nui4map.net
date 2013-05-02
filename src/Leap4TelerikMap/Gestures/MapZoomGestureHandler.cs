using System;
using System.Windows;
using MapUtils.Distance;
using Telerik.Windows.Controls;
using Leap4Map.Gestures;
using Leap4Map.Extensions;

namespace Leap4TelerikMap.Gestures
{

    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
    {

        private RadMap _map;
        private int _startZoomLevel;
        private const float _PixelsDistanteToChangeZoomLevel = 400;

        public override event Action Zooming;
        public override event Action ZoomStarted;
        public override event Action ZoomStopped;

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


        private void DoZoomMap(Leap.Hand hand)
        {
            var radius = hand.SphereRadius - 60;

            var levelDiff = (int)(radius / 3);
            _map.ZoomLevel = levelDiff;
        }

        protected override void RunZooming(Leap.Hand hand)
        {
           // var rightHandCoordinate = rightHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            //var leftHandCoordinate = leftHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            DoZoomMap(hand);
            
            if (Zooming != null)
            {
                Zooming();
            }
        }

        protected override void StartZoom(Leap.Hand hand)
        {
            IsZooming = true;
            //_startRightHandScreenCoord = rightHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            //_startLeftHandScreenCoord = leftHandPoint.ToScreenPoint(_map.ActualWidth, _map.ActualHeight);
            //_startDistance = _startRightHandScreenCoord.DistanceFrom(_startLeftHandScreenCoord);
            _startZoomLevel = _map.ZoomLevel;
           
            if (ZoomStarted != null)
            {
                ZoomStarted();
            }
        }

        protected override void StopZooming()
        {
            if (IsZooming)
            {
                IsZooming = false;
                
                if (ZoomStopped != null)
                {
                    ZoomStopped();
                }
            }
        }

    }

}

