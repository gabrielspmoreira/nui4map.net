using System;
using Leap4TelerikMap.Extensions;
using MapUtils.Structs;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;
using Leap4Map.Gestures;
using NUI4Map.Structs;
using TelerikMapCommons.Extensions;

namespace Leap4TelerikMap.Gestures
{

    public class MapClickGestureHandler : MapClickGestureHandlerBase
    {

        private RadMap _map;

        public override event Action<MapCoord> NUIMapClick;
        public override event Action<MapCoord> MouseMapClick;

        public override object MapComponent
        {
            get
            {
                return _map;
            }
            set
            {
                bool flag = !(value is RadMap);
                if (!flag)
                {
                    _map = value as RadMap;
                    _map.MapMouseClick -= MapMouseClickHandler;
                    _map.MapMouseClick += MapMouseClickHandler;
                }
                else
                {
                    throw new InvalidCastException("Não é uma instância RadMap válida!");
                }
            }
        }


        private void MapMouseClickHandler(object sender, MapMouseRoutedEventArgs eventArgs)
        {
            if (MouseMapClick != null)
            {
                MouseMapClick(eventArgs.Location.ToMapCoord());
            }
        }

        protected override void DoMapClick(Vector3D handPoint)
        {
            Location mapPoint = handPoint.ToTelerikMapLocation(_map);
            if (NUIMapClick != null)
            {
                NUIMapClick(mapPoint.ToMapCoord());
            }                
        }

    }

}

