using System;
using MapUtils.Structs;

namespace NUI4Map.Gestures
{
    public interface IMapClickGestureHandler
    {
        object MapComponent { set; }
        event Action<MapCoord> NUIMapClick;
        event Action<MapCoord> MouseMapClick;
        bool Detect(object frame);
    }
}