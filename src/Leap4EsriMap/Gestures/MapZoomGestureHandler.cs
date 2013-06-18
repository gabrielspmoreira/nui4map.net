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

        protected override void StartZoom(Leap.Frame frame)
        {
            _startMapResolution = _map.Resolution;
            base.StartZoom(frame);
        }

        protected override bool DoZoomMap(Leap.Frame frame)
        {
            var handPosition = frame.Hands[0].PalmPosition.ToVector3D().ToEsriWebMercatorMapPoint(_map);
            var zoomCenter = new MapPoint(handPosition.X, handPosition.Y, _map.SpatialReference);
            var scale = frame.ScaleFactor(StartFrame);

            if (scale >= 1.2 || scale <= 0.95)
            {
                var targetResolution = _startMapResolution / Math.Pow(scale, 8);

                _map.ZoomToResolution(targetResolution, zoomCenter);

                return true;
            }

            return false;
        }


        #endregion
    }
}
