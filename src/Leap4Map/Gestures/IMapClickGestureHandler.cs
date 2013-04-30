using System;
using MapUtils.Structs;

namespace Leap4Map.Gestures
{
    public interface IMapClickGestureHandler
    {
        object MapComponent { set; } 
        event Action<MapCoord> LeapMapClick;
        event Action<MapCoord> MouseMapClick;
        bool Detect(Leap.Frame frame);
    }
}