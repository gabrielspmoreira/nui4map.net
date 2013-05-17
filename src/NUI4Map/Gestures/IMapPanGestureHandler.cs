using System;
using MapUtils.Structs;
using NUI4Map.Structs;

namespace NUI4Map.Gestures
{
    public interface IMapPanGestureHandler
    {
        object MapComponent { set; } 
        bool IsPanning { get; }
        Hand PanningHand { get; }
        event Action<MapCoord> PanStart;
        event Action PanStop;
        event Action<MapCoord> Panning;

        bool Detect(object frame);
    }
}