using System;
using System.Windows;
using MapUtils.Structs;

namespace Kinect4Map.MapUtils
{
    public interface IMapHandler
    {
        event Action MapInitialized;
        UIElement CreateMap();
        void ShowClickedFlag(MapCoord coord, Uri imageUri);
        void ShowTargetFlag(MapCoord coord, Uri imageUri);
        void ClearFlags();
    }
}