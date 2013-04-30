using System;
using System.Windows;
using Kinect4Map.Gestures;
using Kinect4TelerikMap.Extensions;
using MapUtils.Structs;
using Microsoft.Kinect;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Map;

namespace Kinect4TelerikMap.Gestures
{

    public class MapClickGestureHandler : MapClickGestureHandlerBase
    {

        private RadMap _map;

        public override event Action<MapCoord> KinectMapClick;
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

        protected override void DoMapClick(SkeletonPoint handPoint)
        {
            Location mapPoint = handPoint.ToTelerikMapLocation(_map);
            if (KinectMapClick != null)
            {
                KinectMapClick(mapPoint.ToMapCoord());
            }                
        }

    }

}

