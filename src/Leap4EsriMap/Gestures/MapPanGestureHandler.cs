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

        #region Private Methods      

        protected override void DoPan(Vector3D movementVector)
        {
            var deltaX = _map.Extent.Width / 4;
            var deltaY = _map.Extent.Height / 4;
            
            var currentCenter = _map.Extent.GetCenter();
            var nextExtent = new Envelope
            {
                XMin = _map.Extent.XMin - (movementVector.X * deltaX),
                XMax = _map.Extent.XMax - (movementVector.X * deltaX),
                YMin = _map.Extent.YMin - (movementVector.Y * deltaY),
                YMax = _map.Extent.YMax - (movementVector.Y * deltaY)
            };

            _map.PanTo(nextExtent.GetCenter());
        }

        #endregion
    }
}
