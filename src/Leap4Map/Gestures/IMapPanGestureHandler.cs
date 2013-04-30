using System;
using MapUtils.Structs;
using Leap4TelerikMap.Structs;

namespace Leap4Map.Gestures
{
    public interface IMapPanGestureHandler
    {
        object MapComponent { set; } 
        bool IsPanning { get; }
        event Action<MapCoord> PanStart;
        event Action PanStop;
        event Action<MapCoord> Panning;

        bool Detect(Leap.Frame frame);
    }
}