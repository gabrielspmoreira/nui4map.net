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

        protected override void DoPan(Vector3D movementVector)
        {
            var deltaX = _map.GeographicalBounds.Width / 4;
            var deltaY = _map.GeographicalBounds.Height / 4;

            var nextCenter = new Location
            {
                Longitude = _map.Center.Longitude - (movementVector.X * deltaX),
                Latitude = _map.Center.Latitude - (movementVector.Y * deltaY)
            };

            _map.Center = nextCenter;
        }

        

    } 

}

