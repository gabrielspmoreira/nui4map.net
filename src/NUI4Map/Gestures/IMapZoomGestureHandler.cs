using System;

namespace NUI4Map.Gestures
{
    public interface IMapZoomGestureHandler
    {
        object MapComponent { set; } 
        event Action ZoomStarted;
        event Action ZoomStopped;
        event Action Zooming;
        bool IsZooming { get; }

        bool Detect(object frame);
    }
}