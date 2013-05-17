using System;
using System.Windows;
using Leap;
using MapUtils.Distance;
using Telerik.Windows.Controls;
using Leap4Map.Gestures;
using Leap4Map.Extensions;

namespace Leap4TelerikMap.Gestures
{

    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
    {

        private RadMap _map;
        private Frame _startFrame;

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


        private void DoZoomMap(Leap.Frame frame)
        {
            var scale = frame.Hands[0].ScaleFactor(_startFrame);

            if (scale >= 1.1)
            {
                _map.ZoomLevel += 1;
                _startFrame = frame;
            }
            else if (scale <= 0.95)
            {
                _map.ZoomLevel -= 1;
                _startFrame = frame;
            }
        }

        protected override void RunZooming(Leap.Frame frame)
        {
            DoZoomMap(frame);
            
            if (Zooming != null)
            {
                Zooming();
            }
        }

        protected override void StartZoom(Leap.Frame frame)
        {
            IsZooming = true;
            _startFrame = frame;
           
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

