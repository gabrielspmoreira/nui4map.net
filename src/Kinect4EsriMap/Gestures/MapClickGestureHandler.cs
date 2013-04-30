using System;
using Kinect4EsriMap.Extensions;
using Kinect4Map.Gestures;
using MapUtils.Structs;
using Microsoft.Kinect;
using ESRI.ArcGIS.Client;

namespace Kinect4EsriMap.Gestures
{
    public class MapClickGestureHandler : MapClickGestureHandlerBase
    {

        #region Attributes

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

                    _map.MouseClick -= MapMouseClickHandler;
                    _map.MouseClick += MapMouseClickHandler;
                }
                else
                {
                    throw new InvalidCastException("Não é uma instância de Esri Map válida.");
                }
            }
        }

        #endregion

        #region Events
        public override event Action<MapCoord> KinectMapClick;
        public override event Action<MapCoord> MouseMapClick;
        #endregion

        #region Private Methods

        protected override void DoMapClick(SkeletonPoint handPoint)
        {
            var mapPoint = handPoint.ToEsriWebMercatorMapPoint(_map);
            if (KinectMapClick != null)
            {
                KinectMapClick(mapPoint.ToMapCoord());
            }            
        }

        private void MapMouseClickHandler(object sender, Map.MouseEventArgs e)
        {
            if (MouseMapClick != null)
            {
                MouseMapClick(e.MapPoint.ToMapCoord());
            }
        }

        #endregion
    }
}
