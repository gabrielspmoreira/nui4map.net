using System;
using MapUtils.Structs;

namespace NUI4Map.Gestures
{
    public interface IMapPanGestureHandler
    {
        object MapComponent { set; } 
        bool IsPanning { get; }
        event Action<MapCoord> PanStart;
        event Action PanStop;
        event Action<MapCoord> Panning;

        bool Detect(object frame);
    }
}