using System;
using EsriMapCommons.Extensions;
using Leap4EsriMap.Extensions;
using Leap4Map.Gestures;
using MapUtils.Structs;
using ESRI.ArcGIS.Client;
using NUI4Map.Structs;

namespace Leap4EsriMap.Gestures
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
        public override event Action<MapCoord> NUIMapClick;
        public override event Action<MapCoord> MouseMapClick;
        #endregion

        #region Private Methods

        protected override void DoMapClick(Vector3D handPoint)
        {
            var mapPoint = handPoint.ToEsriWebMercatorMapPoint(_map);
            if (NUIMapClick != null)
            {
                NUIMapClick(mapPoint.ToMapCoord());
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
