using System;
using ESRI.ArcGIS.Client;
using Leap;
using Leap4Map.Extensions;
using Leap4Map.Gestures;
using ESRI.ArcGIS.Client.Geometry;
using Leap4EsriMap.Extensions;

namespace Leap4EsriMap.Gestures
{
    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
    {
        #region Attributes

        private double _startMapResolution;

        private Map _map;

        private Frame _startFrame;

        #endregion

        #region Events
        public override event Action ZoomStarted;
        public override event Action ZoomStopped;
        public override event Action Zooming;

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
            _startMapResolution = _map.Resolution;

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




        private void DoZoomMap(Leap.Frame frame)
        {
            var handPosition = frame.Hands[0].PalmPosition.ToVector3D().ToEsriWebMercatorMapPoint(_map);
            var zoomCenter = new MapPoint(handPosition.X, handPosition.Y, _map.SpatialReference);
            var scale = frame.ScaleFactor(_startFrame);

            if (scale >= 1.1 || scale <= 0.95)
            {
                var targetResolution = _startMapResolution / Math.Pow(scale, 8);

                _map.ZoomToResolution(targetResolution, zoomCenter);
                _startFrame = frame;
            }
        }


        #endregion
    }
}
