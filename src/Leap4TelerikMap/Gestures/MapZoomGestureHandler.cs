using System;
using System.Windows;
using Leap;
using MapUtils.Distance;
using Telerik.Windows.Controls;
using Leap4Map.Gestures;
using Leap4Map.Extensions;
using System.Collections;

namespace Leap4TelerikMap.Gestures
{

    public class MapZoomGestureHandler : MapZoomGestureHandlerBase
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
                    throw new InvalidCastException("Não é uma instância de RadMap válida");
                }
            }
        }


        protected override bool DoZoomMap(Leap.Frame frame)
        {
            var scale = frame.Hands[0].ScaleFactor(StartFrame);

            if (scale >= 1.2)
            {
                _map.ZoomLevel += 1;
                return true;
            }
            else if (scale <= 0.95)
            {
                _map.ZoomLevel -= 1;
                return true;
            }

            return false;
        }

        

    }

}

